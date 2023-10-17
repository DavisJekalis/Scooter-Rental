namespace ScooterRental.Exceptions
{
    public class ScooterNotInRentException : Exception
    {
        public ScooterNotInRentException() : base("Scooter is currently not rented")
        {
        }
    }
}
