namespace ScooterRental.Exceptions
{
    public class NoScootersInThisPeriodException : Exception
    {
        public NoScootersInThisPeriodException() : base("No Scooters in this period")
        {
        }
    }
}
