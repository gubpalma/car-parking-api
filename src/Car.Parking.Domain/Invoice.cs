namespace Car.Parking.Domain
{
    public class Invoice
    {
        public string RateName { get; set; }

        public decimal SubTotal { get; set; }

        public decimal TaxTotal { get; set; }

        public string TaxName { get; set; }

        public decimal GrandTotal { get; set; }
    }
}
