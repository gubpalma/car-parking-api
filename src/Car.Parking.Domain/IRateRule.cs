using System;

namespace Car.Parking.Domain
{
    public interface IRateRule
    {
        decimal Checkout(DateTime entryTime, DateTime exitTime);

        bool IsApplicable(DateTime entryTime, DateTime exitTime);

        string Name { get; set; }
    }
}
