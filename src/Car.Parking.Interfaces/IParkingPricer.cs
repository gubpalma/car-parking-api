using System;
using Car.Parking.Domain;

namespace Car.Parking.Interfaces
{
    public interface IParkingPricer
    {
        Invoice CalculateTotals(ParkingEntry entry);

        Invoice CalculateTotals(DateTime entryTime, DateTime? exitTime);
    }
}
