using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.AutoMoq;
using Car.Parking.Domain;
using Car.Parking.Infrastructure.Pricing;
using Car.Parking.Infrastructure.Rules;
using Moq;
using Xunit;

namespace Car.Parking.Tests.Unit
{
    public class ParkingPricerTests
    {
        private readonly TestContext _context;
       
        public ParkingPricerTests() => _context = new TestContext();

        // 03/07/2020 is a Friday
        [Theory]
        [InlineData("03/07/2020 13:00:00", "03/07/2020 14:00:00", 10.00, "Standard Rate")]
        [InlineData("02/07/2020 13:00:00", "02/07/2020 13:55:00", 5.00, "Standard Rate")]
        [InlineData("02/07/2020 08:00:00", "02/07/2020 10:55:00", 15.00, "Standard Rate")]
        [InlineData("03/07/2020 07:50:00", "03/07/2020 15:35:00", 13.00, "Early Bird")]
        [InlineData("03/07/2020 07:50:00", "03/07/2020 15:00:00", 20.00, "Standard Rate")]
        [InlineData("03/07/2020 04:45:00", "03/07/2020 15:35:00", 20.00, "Standard Rate")]
        [InlineData("01/07/2020 18:39:00", "02/07/2020 16:00:00", 6.50, "Night Rate")]
        [InlineData("01/07/2020 18:39:00", "02/07/2020 08:00:00", 20.00, "Standard Rate")]
        [InlineData("30/06/2020 18:39:00", "02/07/2020 08:00:00", 20.00, "Standard Rate")]
        [InlineData("01/07/2020 18:39:00", "03/07/2020 19:00:00", 40.00, "Standard Rate")]
        [InlineData("03/07/2020 23:00:00", "05/07/2020 08:00:00", 10.00, "Weekend Rate")]
        [InlineData("30/06/2020 10:00:00", "05/07/2020 08:00:00", 80.00, "Standard Rate")]
        public void Test_Parking_Pricing(
            string entryDate,
            string exitDate,
            decimal expectedSubtotal,
            string expectedRateName)
        {
            var entryTime = DateTime.Parse(entryDate);
            var exitTime = DateTime.Parse(exitDate);

            _context.ArrangeTaxRate();
            _context.ActCalculateInvoice(entryTime, exitTime);
            _context.AssertInvoiceTotal(expectedSubtotal, expectedRateName);
        }

        [Fact]
        public void Test_Null_Date()
        {
            _context.ArrangeTaxRate();
            Assert.ThrowsAny<Exception>(() => _context.ActCalculateInvoice(DateTime.Now, null));
        }

        private class TestContext
        {
            private readonly ParkingPricer _sut;
            private readonly Mock<ITaxRule> _taxRule;
            private Invoice _result;

            public TestContext()
            {
                var fixture = new Fixture().Customize(new AutoMoqCustomization());

                fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
                    .ToList()
                    .ForEach(b => fixture.Behaviors.Remove(b));

                fixture.Behaviors.Add(new OmitOnRecursionBehavior());

                _taxRule = new Mock<ITaxRule>();
                
                _sut =
                    new ParkingPricer(
                        new List<IRateRule>
                        {
                            new EarlyBirdRateRule(),
                            new NightRateRule(),
                            new WeekendRateRule()
                        }, 
                        new HourlyRateRule(),
                        _taxRule.Object
                    );
            }

            public void ArrangeTaxRate()
            {
                _taxRule
                    .Setup(o => o.Apply(It.IsAny<decimal>()))
                    .Returns(0.0m);
            }
            
            public void ActCalculateInvoice(DateTime entryTime, DateTime? exitTime) => _result = _sut.CalculateTotals(entryTime, exitTime);

            public void AssertInvoiceTotal(decimal expectedSubtotal, string expectedRateName)
            {
                Assert.NotNull(_result);
                Assert.Equal(expectedSubtotal, _result.SubTotal);
                Assert.Equal(expectedRateName, _result.RateName);
            }
        }
    }
}
