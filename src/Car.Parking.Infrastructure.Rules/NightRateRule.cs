using System;
using Car.Parking.Domain;
using Car.Parking.Infrastructure.Rules.Extensions;

namespace Car.Parking.Infrastructure.Rules
{
    public class NightRateRule : IRateRule
    {
        public decimal Checkout(DateTime entryTime, DateTime exitTime)
        {
            return 6.50m;
        }

        public bool IsApplicable(DateTime entryTime, DateTime exitTime)
        {
            var applicable = true;

            applicable &= entryTime.IsWeekday();

            applicable &= entryTime.TimeOfDay >= new TimeSpan(18, 0, 0);
            applicable &= entryTime.TimeOfDay <= new TimeSpan(23, 59, 59);

            applicable &= exitTime.TimeOfDay >= new TimeSpan(15, 30, 0);
            applicable &= exitTime.TimeOfDay <= new TimeSpan(23, 30, 0);

            /* Assuming that night rate only applies if you are leaving the next day */

            applicable &= (exitTime - entryTime).Days < 1;

            return applicable;
        }

        public string Name { get; set; } = "Night Rate";
    }
}