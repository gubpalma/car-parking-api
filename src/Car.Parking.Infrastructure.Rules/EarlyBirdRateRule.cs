using System;
using Car.Parking.Domain;

namespace Car.Parking.Infrastructure.Rules
{
    public class EarlyBirdRateRule : IRateRule
    {
        public decimal Checkout(DateTime entryTime, DateTime exitTime)
        {
            return 13.00m;
        }

        public bool IsApplicable(DateTime entryTime, DateTime exitTime)
        {
            var applicable = true;

            applicable &= entryTime.TimeOfDay >= new TimeSpan(6, 0, 0);
            applicable &= entryTime.TimeOfDay <= new TimeSpan(9, 0, 0);

            applicable &= exitTime.TimeOfDay >= new TimeSpan(15, 30, 0);
            applicable &= exitTime.TimeOfDay <= new TimeSpan(23, 30, 0);

            return applicable;
        }

        public string Name { get; set; } = "Early Bird";
    }
}
