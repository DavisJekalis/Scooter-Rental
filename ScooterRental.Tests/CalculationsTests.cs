using FluentAssertions;
using Moq.AutoMock;
using ScooterRental.Exceptions;
using ScooterRental.Interfaces;

namespace ScooterRental.Tests
{
    [TestClass]
    public class CalculationsTests
    {
        private const string DEFAULT_SCOOTER_ID = "1";
        private const decimal DEFAULT_PRICE_PER_MINUTE = 1m;
        private RentedScooter DEFAULT_RENTED_SCOOTER;
        private ICalculations _calculations;
        private AutoMocker _mocker;
        private List<RentedScooter> _rentedScooters;

        [TestInitialize]
        public void Setup()
        {
            _mocker = new AutoMocker();
            _calculations = new Calculations(_mocker.GetMock<IScooterService>().Object);
            DEFAULT_RENTED_SCOOTER = new RentedScooter(DEFAULT_SCOOTER_ID, DateTime.Now.AddMinutes(-10))
                { RentEnd = DateTime.Now };
            _rentedScooters = new List<RentedScooter>();
        }

        [TestMethod]
        public void CalculateRentalCost_ForRentedScooter_PriceEqualsRentTime()
        {
            var price = _calculations.CalculateRentalCost(DEFAULT_RENTED_SCOOTER, DEFAULT_PRICE_PER_MINUTE);

            price.Should().Be((decimal)(10));
        }

        [TestMethod]
        public void CalculateRentalCost_ScooterRentedfor1Day_PriceEquals20()
        {
            RentedScooter rentedScooter = new RentedScooter(DEFAULT_SCOOTER_ID, DateTime.Now.AddDays(-2).Date)
                { RentEnd = DateTime.Now.AddDays(-1).Date };

            var price = _calculations.CalculateRentalCost(rentedScooter, DEFAULT_PRICE_PER_MINUTE);

            price.Should().Be(20m);
        }

        [TestMethod]
        public void CalculateIncomeForPeriod_ScooterWithNoEndTime_ReturnsDecimal()
        {
            var scooter = new Scooter(DEFAULT_SCOOTER_ID, DEFAULT_PRICE_PER_MINUTE);
            _mocker.GetMock<IScooterService>().Setup(s => s.GetScooterById(DEFAULT_SCOOTER_ID)).Returns(scooter);
            _rentedScooters.Add(new RentedScooter(DEFAULT_SCOOTER_ID, DateTime.Now.AddDays(-2).Date));
            var result = _calculations.CalculateIncomeForPeriod(null, true, _rentedScooters);

            result.Should().BeOfType(typeof(decimal));
        }

        [TestMethod]
        public void CalculateIncomeForPeriod_ScooterRentedForSpecificYear_ReturnsDecimal()
        {
            var scooter = new Scooter(DEFAULT_SCOOTER_ID, DEFAULT_PRICE_PER_MINUTE);
            _mocker.GetMock<IScooterService>().Setup(s => s.GetScooterById(DEFAULT_SCOOTER_ID)).Returns(scooter);
            _rentedScooters.Add(new RentedScooter(DEFAULT_SCOOTER_ID, 
                DateTime.Now.AddYears(-1)){RentEnd = DateTime.Now.AddYears(-1).AddDays(-2)});

            var result = _calculations.CalculateIncomeForPeriod(2022, true, _rentedScooters);

            result.Should().BeOfType(typeof(decimal));
        }

        [TestMethod]
        public void CalculateIncomeForPeriod_ForPeriodWithNoScooters_ThrowsNoScootersInThisPeriodException()
        {
            _rentedScooters.Add(new RentedScooter(DEFAULT_SCOOTER_ID,
                    DateTime.Now.AddYears(-1))
                { RentEnd = DateTime.Now.AddYears(-1).AddDays(-2) });

            Action action = () => _calculations.CalculateIncomeForPeriod(2024, true, _rentedScooters);

            action.Should().Throw<NoScootersInThisPeriodException>();
        }

        [TestMethod]
        public void CalculateIncomeForPeriod_RemovesUnwantedEntriesFromList_ReturnsDecimal()
        {
            var scooter = new Scooter(DEFAULT_SCOOTER_ID, DEFAULT_PRICE_PER_MINUTE);
            _mocker.GetMock<IScooterService>().Setup(s => s.GetScooterById(DEFAULT_SCOOTER_ID)).Returns(scooter);

            _rentedScooters.Add(new RentedScooter(DEFAULT_SCOOTER_ID,
                    DateTime.Now.AddYears(-1))
                { RentEnd = DateTime.Now.AddYears(-1).AddDays(-2) });

            _rentedScooters.Add(new RentedScooter(DEFAULT_SCOOTER_ID, DateTime.Now){ RentEnd = DateTime.Now });

            var result = _calculations.CalculateIncomeForPeriod(2022, true, _rentedScooters);

            result.Should().BeOfType(typeof(decimal));
        }

        [TestMethod]
        public void CalculateIncomeForPeriod_AddsEndDateToUnfinishedEntries_ReturnsDecimal()
        {
            var scooter = new Scooter(DEFAULT_SCOOTER_ID, DEFAULT_PRICE_PER_MINUTE);
            _mocker.GetMock<IScooterService>().Setup(s => s.GetScooterById(DEFAULT_SCOOTER_ID)).Returns(scooter);

            _rentedScooters.Add(new RentedScooter(DEFAULT_SCOOTER_ID, DateTime.Now.AddYears(-1)) 
                { RentEnd = DateTime.Now.AddYears(-1).AddDays(-2) });

            _rentedScooters.Add(new RentedScooter(DEFAULT_SCOOTER_ID, DateTime.Now));

            var result = _calculations.CalculateIncomeForPeriod(2022, true, _rentedScooters);

            result.Should().BeOfType(typeof(decimal));
        }

        [TestMethod]
        public void CalculateIncomeForPeriod_OnlyForSpecificYear_ReturnsDecimal()
        {
            var scooter = new Scooter(DEFAULT_SCOOTER_ID, DEFAULT_PRICE_PER_MINUTE);
            _mocker.GetMock<IScooterService>().Setup(s => s.GetScooterById(DEFAULT_SCOOTER_ID)).Returns(scooter);

            _rentedScooters.Add(new RentedScooter(DEFAULT_SCOOTER_ID, DateTime.Now.AddYears(-1))
                { RentEnd = DateTime.Now.AddYears(-1).AddDays(-2) });

            _rentedScooters.Add(new RentedScooter(DEFAULT_SCOOTER_ID, DateTime.Now));

            var result = _calculations.CalculateIncomeForPeriod(2022, false, _rentedScooters);

            result.Should().BeOfType(typeof(decimal));
        }

        [TestMethod]
        public void CalculateIncomeForPeriod_NoCriteria_ReturnsDecimal()
        {
            var scooter = new Scooter(DEFAULT_SCOOTER_ID, DEFAULT_PRICE_PER_MINUTE);
            _mocker.GetMock<IScooterService>().Setup(s => s.GetScooterById(DEFAULT_SCOOTER_ID)).Returns(scooter);

            _rentedScooters.Add(new RentedScooter(DEFAULT_SCOOTER_ID, DateTime.Now.AddYears(-1))
                { RentEnd = DateTime.Now.AddYears(-1).AddDays(-2) });

            _rentedScooters.Add(new RentedScooter(DEFAULT_SCOOTER_ID, DateTime.Now));

            var result = _calculations.CalculateIncomeForPeriod(null, false, _rentedScooters);

            result.Should().BeOfType(typeof(decimal));
        }
    }
}
