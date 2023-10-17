using ScooterRental.Exceptions;
using ScooterRental.Interfaces;

namespace ScooterRental
{
    public class RentedScooterService : IRentedScooterService
    {
        private readonly List<RentedScooter> _rentedScooterList;

        public RentedScooterService(List<RentedScooter> rentedScooterList)
        {
            _rentedScooterList = rentedScooterList;
        }

        public void StartRent(string id)
        {
            _rentedScooterList.Add(new RentedScooter(id, DateTime.Now));
        }

        public RentedScooter StopRent(string id)
        {
            var rentalRecord = _rentedScooterList.FirstOrDefault(s =>
                s.Id == id && !s.RentEnd.HasValue);

            if (rentalRecord == null)
            {
                throw new NoRentalRecordException();
            }

            rentalRecord.RentEnd = DateTime.Now;

            return rentalRecord;
        }

        public List<RentedScooter> GetRentalRecords()
        {
            if (_rentedScooterList.Count == 0)
            {
                throw new NoScootersRentedException();
            }

            return _rentedScooterList;
        }
    }
}
