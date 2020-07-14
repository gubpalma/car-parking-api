using System;

namespace Car.Parking.Infrastructure.Rules
{
    public class HourlyRateRule : IDefaultRateRule
    {
        private const decimal HourlyRate = 5.0m;

        public decimal Checkout(DateTime entryTime, DateTime exitTime)
        {
            var timespan = exitTime - entryTime;

            if (timespan.Days <= 1)
            {
                return timespan.Hours < 3 ? (timespan.Hours + 1) * HourlyRate : HourlyRate * 4;
            }

            return HourlyRate * 4 * timespan.Days;
        }

        public bool IsApplicable(DateTime entryTime, DateTime exitTime) => false;

        public string Name { get; set; } = "Standard Rate";
    }
}
