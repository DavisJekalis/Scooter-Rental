namespace ScooterRental.Exceptions
{
    public class EmptyScooterStorageException : Exception
    {
        public EmptyScooterStorageException() : base("No Scooters in Storage")
        {
        }
    }
}
