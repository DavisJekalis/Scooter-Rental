using ScooterRental.Interfaces;
using ScooterRental.Exceptions;

namespace ScooterRental
{
    public class RentalCompany : IRentalCompany
    {
        private readonly IScooterService _scooterService;
        private readonly ICalculations _calculations;
        private readonly IRentedScooterService _rentedScooterService;

        public string Name { get; }

        public RentalCompany(string name, IScooterService scooterService, 
            IRentedScooterService rentedScooterService, ICalculations calculations)
        {
            Name = name;
            _scooterService = scooterService;
            _rentedScooterService = rentedScooterService;
            _calculations = calculations;
        }

        public void StartRent(string id)
        {
            var scooter = _scooterService.GetScooterById(id);

            if (scooter.IsRented)
            {
                throw new ScooterAlreadyRentedException();
            }

            scooter.IsRented = true;
            _rentedScooterService.StartRent(id);
        }

        public decimal EndRent(string id)
        {
            var scooter = _scooterService.GetScooterById(id);

            if (!scooter.IsRented)
            {
                throw new ScooterNotInRentException();
            }

            var rentalRecord = _rentedScooterService.StopRent(id);
            scooter.IsRented = false;

            return _calculations.CalculateRentalCost(rentalRecord, scooter.PricePerMinute);
        }

        public decimal CalculateIncome(int? year, bool includeNotCompletedRentals)
        {
            return _calculations.CalculateIncomeForPeriod(year, includeNotCompletedRentals,
                _rentedScooterService.GetRentalRecords());
        }
    }
}
