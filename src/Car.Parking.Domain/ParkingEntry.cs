using System;

namespace Car.Parking.Domain
{
    public class ParkingEntry
    {
        public Guid ParkingEntryId { get; set; }

        public string ParkingRegistration { get; set; }

        public DateTime EntryTime { get; set; }

        public DateTime? ExitTime { get; set; }
    }
}
