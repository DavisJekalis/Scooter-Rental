namespace ScooterRental.Interfaces;

public interface IRentedScooterService
{
    public void StartRent(string id);

    public RentedScooter StopRent(string id);

    public List<RentedScooter> GetRentalRecords();
}