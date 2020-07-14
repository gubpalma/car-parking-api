using System.Collections.Generic;
using Car.Parking.Domain;
using Car.Parking.Interfaces;

namespace Car.Parking.Infrastructure.Repository
{
    public class ParkingEntryRepository : BaseRepository<ParkingEntry>, IRepository<ParkingEntry>
    {
        private static readonly List<ParkingEntry> Table = new List<ParkingEntry>();

        public ParkingEntryRepository() : base(Table, o => o.ParkingEntryId) { }
    }
}