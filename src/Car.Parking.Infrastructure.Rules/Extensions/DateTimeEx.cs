using System;

namespace Car.Parking.Infrastructure.Rules.Extensions
{
    public static class DateTimeEx
    {
        public static bool IsWeekend(this DateTime value)
        {
            var day = value.DayOfWeek;

            return day == DayOfWeek.Saturday || day == DayOfWeek.Sunday;
        }

        public static bool IsWeekday(this DateTime value) => !IsWeekend(value);
    }
}
