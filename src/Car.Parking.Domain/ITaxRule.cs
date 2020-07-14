namespace Car.Parking.Domain
{
    public interface ITaxRule
    {
        decimal Apply(decimal subTotal);

        string Name { get; set; }
    }
}