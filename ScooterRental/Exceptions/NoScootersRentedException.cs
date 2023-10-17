namespace ScooterRental.Exceptions
{
    public class NoScootersRentedException : Exception
    {
        public NoScootersRentedException() : base("No Scooters were ever rented")
        {
        }
    }
}
