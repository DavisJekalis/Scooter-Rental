using ScooterRental.Exceptions;
using ScooterRental.Interfaces;

namespace ScooterRental
{
    public class Calculations : ICalculations
    {
        private readonly IScooterService _scooterService;

        public Calculations(IScooterService scooterService)
        {
            _scooterService = scooterService;
        }

        public decimal CalculateRentalCost(RentedScooter rentedScooter, decimal price)
        {
            decimal result = 0;

            var totalPrice = TimeSpanToMinutesInt(rentedScooter.RentEnd.Value - rentedScooter.RentStart) * price;

            TimeSpan totalTime = (rentedScooter.RentEnd.Value - rentedScooter.RentStart);
            TimeSpan timeTilMidnight = (rentedScooter.RentStart.AddDays(1).Date - rentedScooter.RentStart);

            var newDay = new DateTime();
            var days = 1;

            while (true)
            {
                if (totalTime >= timeTilMidnight)
                {
                    totalTime -= timeTilMidnight;

                    result = TimeSpanToMinutesInt(timeTilMidnight) * price > 20 ?
                        result += 20 :
                        result += TimeSpanToMinutesInt(timeTilMidnight) * price;

                    newDay = rentedScooter.RentStart.AddDays(days).Date;
                    timeTilMidnight = (rentedScooter.RentStart.AddDays(days + 1).Date - newDay);

                    days++;
                }
                else if (result == 20)
                {
                    break;
                }
                else if (days > 1)
                {
                    result = TimeSpanToMinutesInt(totalTime) * price + price > 20 ?
                       result += 20 :
                       result += TimeSpanToMinutesInt(totalTime) * price + price;
                    break;
                }
                else
                {
                    result += totalPrice;
                    break;
                }
            }

            return result;
        }

        public decimal CalculateIncomeForPeriod(int? year, bool includeNotCompletedRentals,
            List<RentedScooter> rentedScooterList)
        {
            var result = 0m;
            var choice = year.HasValue && includeNotCompletedRentals ? 0 :
                includeNotCompletedRentals ? 1 :
                year.HasValue ? 2 : 3;


            switch (choice)
            {
                case 0:
                    SetEndTimeToNow(rentedScooterList);
                    rentedScooterList.RemoveAll(scooter => scooter.RentEnd.Value.Year != year);
                    result = GetCostForFilteredPeriod(rentedScooterList);
                    break;
                case 1:
                    SetEndTimeToNow(rentedScooterList);
                    result = GetCostForFilteredPeriod(rentedScooterList);
                    break;
                case 2:
                    rentedScooterList.RemoveAll(scooter => !scooter.RentEnd.HasValue);
                    rentedScooterList.RemoveAll(scooter => scooter.RentEnd.Value.Year != year);
                    result = GetCostForFilteredPeriod(rentedScooterList);
                    break;
                case 3:
                    rentedScooterList.RemoveAll(scooter => !scooter.RentEnd.HasValue);
                    result = GetCostForFilteredPeriod(rentedScooterList);
                    break;
            }

            return result;
        }

        private decimal GetCostForFilteredPeriod(List<RentedScooter> filteredScooterList)
        {
            var result = 0m;

            if (filteredScooterList.Count == 0)
            {
                throw new NoScootersInThisPeriodException();
            }

            foreach (var scooter in filteredScooterList)
            {
                result += CalculateRentalCost(scooter, _scooterService.GetScooterById(scooter.Id).PricePerMinute);
            }

            return result;
        }

        private List<RentedScooter> SetEndTimeToNow(List<RentedScooter> scooterList)
        {
            foreach (var scooter in scooterList.Where(scooter => !scooter.RentEnd.HasValue))
            {
                scooter.RentEnd = DateTime.Now;
            }

            return scooterList;
        }
        private int TimeSpanToMinutesInt(TimeSpan date)
        {
            return date.Days * 24 * 60 + date.Hours * 60 + date.Minutes;
        }
    }
}
