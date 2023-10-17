namespace ScooterRental.Exceptions
{
    public class NoRentalRecordException : Exception
    {
        public NoRentalRecordException() : base("No Such Rental record exists")
        {
        }
    }
}
