using ScooterRental.Exceptions;
using ScooterRental.Interfaces;

namespace ScooterRental
{
    public class ScooterService : IScooterService
    {
        private readonly List<Scooter> _scooters;

        public ScooterService(List<Scooter> scooterStorage)
        {
            _scooters = scooterStorage;
        }

        public void AddScooter(string id, decimal pricePerMinute)
        {
            CheckInvalidId(id);

            if (_scooters.Any(s => s.Id == id))
            {
                throw new DuplicateScooterException();
            }

            if (pricePerMinute <= 0)
            {
                throw new NegativePriceException();
            }

            _scooters.Add(new Scooter(id, pricePerMinute));
        }

        public Scooter GetScooterById(string scooterId)
        {
            CheckInvalidId(scooterId);
            CheckScooterExistance(scooterId);

            return _scooters.Find(scooter => scooter.Id == scooterId);
        }

        public IList<Scooter> GetScooters()
        {
            if(_scooters.Count == 0)
            {
                throw new EmptyScooterStorageException();
            }

            return _scooters;
        }

        public void RemoveScooter(string id)
        {
            CheckInvalidId(id);
            CheckScooterExistance(id);

            var scooterToRemove = _scooters.Find(scooter => scooter.Id == id);
            _scooters.Remove(scooterToRemove);
        }

        private void CheckInvalidId(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new InvalidIdException();
            }
        }

        private void CheckScooterExistance(string id)
        {
            if (!_scooters.Any(scooter => scooter.Id == id))
            {
                throw new NonExistingScooterException();
            }
        }
    }
}
