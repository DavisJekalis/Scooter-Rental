namespace ScooterRental.Exceptions
{
    public class NegativePriceException : Exception
    {
        public NegativePriceException() : base("Price cannot be negative")
        {
        }
    }
}
