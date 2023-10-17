namespace ScooterRental.Interfaces;

public interface ICalculations
{
    decimal CalculateRentalCost(RentedScooter rentedScooter, decimal price);

    decimal CalculateIncomeForPeriod(int? year, bool includeNotCompletedRentals,
        List<RentedScooter> filteredList);
}