using Car.Parking.Domain;

namespace Car.Parking.Infrastructure.Rules.Tax
{
    public class GstTaxRule: ITaxRule
    {
        private const decimal GstRate = 0.1m;

        public decimal Apply(decimal subTotal)
        {
            return subTotal * GstRate;
        }

        public string Name { get; set; } = "GST";
    }
}
