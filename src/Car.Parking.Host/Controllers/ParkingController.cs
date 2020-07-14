using System;
using System.Collections.Generic;
using System.Linq;
using Car.Parking.Domain;
using Car.Parking.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Car.Parking.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingController : ControllerBase
    {
        private readonly IRepository<ParkingEntry> _parkingEntryRepository;
        private readonly IParkingPricer _orderPricer;

        public ParkingController(
            IRepository<ParkingEntry> parkingEntryRepository,
            IParkingPricer orderPricer)
        {
            _parkingEntryRepository = parkingEntryRepository;
            _orderPricer = orderPricer;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ParkingEntry>> Get()
        {
            return
                _parkingEntryRepository
                    .Query()
                    .ToList();
        }

        [HttpPost("Enter")]
        public ActionResult<ParkingEntry> Enter([FromBody] string registration)
        {
            var value = new ParkingEntry
            {
                ParkingEntryId = Guid.NewGuid(),
                ParkingRegistration = registration,
                EntryTime = DateTime.Now
            };

            return
                _parkingEntryRepository
                    .AddOrUpdate(value);
        }

        [HttpPost("Checkout")]
        public ActionResult<Invoice> Checkout([FromBody] string registration)
        {
            var parkingEntry =
                _parkingEntryRepository
                    .Query()
                    .FirstOrDefault(o => o.ParkingRegistration == registration && !o.ExitTime.HasValue);

            if (parkingEntry == null)
                return new NotFoundResult();

            parkingEntry.ExitTime = DateTime.Now;

            var result = _orderPricer.CalculateTotals(parkingEntry);

            return result;
        }

        [HttpPost("Exit")]
        public ActionResult<ParkingEntry> Exit([FromBody] string registration)
        {
            var parkingEntry =
                _parkingEntryRepository
                    .Query()
                    .FirstOrDefault(o => o.ParkingRegistration == registration && !o.ExitTime.HasValue);

            if (parkingEntry == null)
                return new NotFoundResult();

            parkingEntry.ExitTime = DateTime.Now;

            return
                _parkingEntryRepository
                    .AddOrUpdate(parkingEntry);
        }
    }
}