namespace ScooterRental.Exceptions
{
    public class NonExistingScooterException : Exception
    {
        public NonExistingScooterException() : base("Such ID Doesn't belong to any Scooter")
        {
        }
    }
}
