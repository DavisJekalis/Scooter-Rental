using FluentAssertions;
using FluentAssertions.Extensions;
using Moq.AutoMock;
using ScooterRental.Exceptions;
using ScooterRental.Interfaces;

namespace ScooterRental.Tests
{
    [TestClass]
    public class RentalCompanyTests
    {
        private const string DEFAULT_COMPANY_NAME = "default";
        private const string DEFAULT_SCOOTER_ID = "1";
        private const decimal DEFAULT_PRICE_PER_MINUTE = 0.01m;
        private IRentalCompany _rentalCompany;
        private AutoMocker _mocker;


        [TestInitialize]
        public void Setup()
        {
            _mocker = new AutoMocker();
            _rentalCompany = new RentalCompany(DEFAULT_COMPANY_NAME, _mocker.GetMock<IScooterService>().Object,
                _mocker.GetMock<IRentedScooterService>().Object, _mocker.GetMock<ICalculations>().Object);
        }

        [TestMethod]
        public void RentalCompany_Name()
        {
            var companyName = _rentalCompany.Name;
            companyName.Should().Be(DEFAULT_COMPANY_NAME);
        }

        [TestMethod]
        public void StartRent_ScooterWhichIsRented_ThrowsScooterAlreadyRentedException()
        {
            var scooter = new Scooter(DEFAULT_SCOOTER_ID, DEFAULT_PRICE_PER_MINUTE){IsRented = true};
            _mocker.GetMock<IScooterService>().Setup(s => s.GetScooterById("1")).Returns(scooter);
            Action action = () => _rentalCompany.StartRent(DEFAULT_SCOOTER_ID);

            action.Should().Throw<ScooterAlreadyRentedException>();
        }

        [TestMethod]
        public void EndRent_ForUnRentedScooter_ThrowsScooterNotInRentException()
        {

            var scooter = new Scooter(DEFAULT_SCOOTER_ID, DEFAULT_PRICE_PER_MINUTE);
            _mocker.GetMock<IScooterService>().Setup(s => s.GetScooterById("1")).Returns(scooter);
            Action action = () => _rentalCompany.EndRent(DEFAULT_SCOOTER_ID);

            action.Should().Throw<ScooterNotInRentException>();
        }

        [TestMethod]
        public void EndRent_DefaultScooter_ChangesIsRentedStatus()
        {
            var scooter = new Scooter(DEFAULT_SCOOTER_ID, DEFAULT_PRICE_PER_MINUTE) { IsRented = true };
            var rentedScooter = new RentedScooter(DEFAULT_SCOOTER_ID, DateTime.Now.AddMinutes(-10));
            _mocker.GetMock<IScooterService>().Setup(s => s.GetScooterById(DEFAULT_SCOOTER_ID)).Returns(scooter);
            _mocker.GetMock<IRentedScooterService>().Setup(r => r.StopRent(DEFAULT_SCOOTER_ID))
                .Returns(rentedScooter);

            var result = _rentalCompany.EndRent(DEFAULT_SCOOTER_ID);
            scooter.IsRented.Should().Be(false);
            result.Should().BeOfType(typeof(decimal));
        }
    }
}
