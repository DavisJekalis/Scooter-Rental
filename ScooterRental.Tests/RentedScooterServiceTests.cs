using FluentAssertions;
using Moq.AutoMock;
using ScooterRental.Exceptions;
using ScooterRental.Interfaces;

namespace ScooterRental.Tests
{
    [TestClass]
    public class RentedScooterServiceTests
    {
        private const string DEFAULT_SCOOTER_ID = "1";
        private IRentedScooterService _rentedScooterService;
        private List<RentedScooter> _rentedScooters;

        [TestInitialize]
        public void Setup()
        {
            _rentedScooters = new List<RentedScooter>();
            _rentedScooterService = new RentedScooterService(_rentedScooters);
        }

        [TestMethod]
        public void RentedScooterService_Constructor()
        {
            _rentedScooterService.Should().NotBeNull();
        }

        [TestMethod]
        public void StartRent_WithId_NewRentedScooterEntry()
        {
            _rentedScooterService.StartRent(DEFAULT_SCOOTER_ID);
            _rentedScooters.First().Should().BeOfType(typeof(RentedScooter));
        }

        [TestMethod]
        public void StartRent_WithId_AddsEntryInList()
        {
            _rentedScooterService.StartRent(DEFAULT_SCOOTER_ID);
            _rentedScooters.Count.Should().Be(1);
        }

        [TestMethod]
        public void StartRent_WithId_AddsDateTime()
        {
            _rentedScooterService.StartRent(DEFAULT_SCOOTER_ID);
            _rentedScooters.First().RentStart.Minute.Should().Be(DateTime.Now.Minute);
        }

        [TestMethod]
        public void StopRent_NonExistingId_ThrowsNoRentalRecordException()
        {
            Action action = () => _rentedScooterService.StopRent(DEFAULT_SCOOTER_ID);
            action.Should().Throw<NoRentalRecordException>();
        }

        [TestMethod]
        public void StopRent_WithId_AddsEndRentValue()
        {
            _rentedScooters.Add(new RentedScooter(DEFAULT_SCOOTER_ID, DateTime.Now));
            _rentedScooterService.StopRent(DEFAULT_SCOOTER_ID);

            _rentedScooters.First().RentEnd.Should().HaveValue();
            _rentedScooters.First().RentEnd.Value.Minute.Should().Be(DateTime.Now.Minute);
        }

        [TestMethod]
        public void StopRent_WithId_ReturnsRentedScooter()
        {
            _rentedScooters.Add(new RentedScooter(DEFAULT_SCOOTER_ID, DateTime.Now));
            var rentedScooter = _rentedScooterService.StopRent(DEFAULT_SCOOTER_ID);

            rentedScooter.Should().BeOfType(typeof(RentedScooter));
        }

        [TestMethod]
        public void GetRentalRecords_EmptyRecordList_ThrowsNoScootersRentedException()
        {
            Action action = () => _rentedScooterService.GetRentalRecords();
            action.Should().Throw<NoScootersRentedException>();
        }

        [TestMethod]
        public void GetRentalRecords_WithEntryInList_ReturnsRentedScooterList()
        {
            _rentedScooters.Add(new RentedScooter(DEFAULT_SCOOTER_ID, DateTime.Now));
            var rentedScooters = _rentedScooterService.GetRentalRecords();
            rentedScooters.Should().BeOfType(typeof(List<RentedScooter>));
        }
    }
}
