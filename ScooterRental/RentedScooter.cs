namespace ScooterRental
{
    public class RentedScooter
    {
        public RentedScooter(string id, DateTime startTime)
        {
            Id = id;
            RentStart = startTime;
        }

        public string Id { get; }

        public DateTime RentStart { get; }

        public DateTime? RentEnd { get; set; }

    }
}
