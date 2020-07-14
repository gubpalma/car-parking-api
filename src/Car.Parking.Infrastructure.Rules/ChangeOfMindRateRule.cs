using System;
using Car.Parking.Domain;

namespace Car.Parking.Infrastructure.Rules
{
    public class ChangeOfMindRateRule : IRateRule
    {
        public decimal Checkout(DateTime entryTime, DateTime exitTime)
        {
            return 0.00m;
        }

        public bool IsApplicable(DateTime entryTime, DateTime exitTime)
        {
            var applicable = true;

            applicable &= (exitTime - entryTime).Minutes <= 5;

            return applicable;
        }

        public string Name { get; set; } = "Change of mind";
    }
}