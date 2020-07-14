using System;
using Car.Parking.Domain;
using Car.Parking.Infrastructure.Rules.Extensions;

namespace Car.Parking.Infrastructure.Rules
{
    public class WeekendRateRule : IRateRule
    {
        public decimal Checkout(DateTime entryTime, DateTime exitTime)
        {
            return 10.00m;
        }

        public bool IsApplicable(DateTime entryTime, DateTime exitTime)
        {
            var applicable = true;

            /*

            As per instruction:

                "If a customer enters the car park before midnight on Friday, and
                if they qualify for Night rate on a Saturday morning,
                then the program should charge the night rate instead of weekend rate."
            
            I am running with the assumption that this only applies to that Friday prior.
            Otherwise a car could stay for an indeterminate number of days and only pay the weekend rate
            provided they exited on a weekend.

            */

            applicable &= entryTime.IsWeekend() || entryTime.DayOfWeek == DayOfWeek.Friday;
            applicable &= exitTime.IsWeekend();
            applicable &= (exitTime - entryTime).Days < 3;

            return applicable;
        }

        public string Name { get; set; } = "Weekend Rate";
    }
}