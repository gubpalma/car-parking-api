using System;
using System.Collections.Generic;
using System.Linq;
using Car.Parking.Domain;
using Car.Parking.Infrastructure.Rules;
using Car.Parking.Interfaces;

namespace Car.Parking.Infrastructure.Pricing
{
    public class ParkingPricer : IParkingPricer
    {
        private readonly IEnumerable<IRateRule> _pricingRules;
        private readonly IDefaultRateRule _defaultPricingRule;
        private readonly ITaxRule _taxRule;

        public ParkingPricer(
            IEnumerable<IRateRule> pricingRules,
            IDefaultRateRule defaultPricingRule,
            ITaxRule taxRule)
        {
            _pricingRules = pricingRules;
            _defaultPricingRule = defaultPricingRule;
            _taxRule = taxRule;
        }

        public Invoice CalculateTotals(DateTime entryTime, DateTime? exitTime)
        {
            if (!exitTime.HasValue || exitTime.Value < entryTime)
                throw new Exception("Invalid times supplied");

            var pricingRules =
                _pricingRules
                    .Where(o => o.IsApplicable(entryTime, exitTime.Value))
                    .ToList();

            if (!pricingRules.Any())
                pricingRules = new List<IRateRule> {_defaultPricingRule};

            var invoices =
                pricingRules
                    .Select(o =>
                    {
                        var subTotal = o.Checkout(entryTime, exitTime.Value);
                        var taxTotal = _taxRule.Apply(subTotal);

                        return new Invoice
                        {
                            RateName = o.Name,
                            SubTotal = subTotal,
                            TaxTotal = taxTotal,
                            TaxName = _taxRule.Name,
                            GrandTotal = subTotal + taxTotal
                        };
                    })
                    .ToList();

            var lowestTotal =
                invoices
                    .Min(o => o.GrandTotal);

            var lowestInvoice =
                invoices
                    .First(o => o.GrandTotal == lowestTotal);

            return lowestInvoice;
        }

        public Invoice CalculateTotals(ParkingEntry entry)
        {
            return CalculateTotals(entry.EntryTime, entry.ExitTime);
        }
    }
}
