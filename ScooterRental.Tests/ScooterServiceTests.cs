using FluentAssertions;
using ScooterRental.Exceptions;

namespace ScooterRental.Tests
{
    [TestClass]
    public class ScooterServiceTests
    {
        private ScooterService _scooterService;
        private List<Scooter> _scooterStorage;
        private const string DEFAULT_SCOOTER_ID = "1";
        private const decimal DEFAULT_PRICE_PER_MINUTE = 0.2m;

        [TestInitialize]
        public void Setup()
        {
            _scooterStorage = new List<Scooter>();
            _scooterService = new ScooterService(_scooterStorage);
        }

        [TestMethod]
        public void AddScooter_WithIdAndPricePerMinute_ScooterAdded()
        {
            _scooterService.AddScooter(DEFAULT_SCOOTER_ID, DEFAULT_PRICE_PER_MINUTE);

            _scooterStorage.Count.Should().Be(1);
        }

        [TestMethod]
        public void AddScooter_WithId1AndPricePerMinute1_ScooterAddedWithId1AndPrice1()
        {
            _scooterService.AddScooter(DEFAULT_SCOOTER_ID, 1m);

            var scooter = _scooterStorage.First();

            scooter.Id.Should().Be(DEFAULT_SCOOTER_ID);
            scooter.PricePerMinute.Should().Be(1m);
        }

        [TestMethod]
        public void AddScooter_DuplicateScooter_ThrowsDuplicateScooterException()
        {
            _scooterStorage.Add(new Scooter(DEFAULT_SCOOTER_ID, DEFAULT_PRICE_PER_MINUTE));

            Action action = () => _scooterService.AddScooter(DEFAULT_SCOOTER_ID, DEFAULT_PRICE_PER_MINUTE);
            action.Should().Throw<DuplicateScooterException>();
        }

        [TestMethod]
        public void AddScooter_WithEmptyID_ThrowsInvalidIdException()
        {
            Action action = () => _scooterService.AddScooter("", DEFAULT_PRICE_PER_MINUTE);
            action.Should().Throw<InvalidIdException>();
        }

        [TestMethod]
        public void AddScooter_WithNegativePrice_ThrowsNegativePriceException()
        {
            Action action = () => _scooterService.AddScooter(DEFAULT_SCOOTER_ID, -1);
            action.Should().Throw<NegativePriceException>();
        }

        [TestMethod]
        public void GetScooterById_AddAndFindScooter_ReturnMatchingScooter()
        {
            _scooterStorage.Add(new Scooter(DEFAULT_SCOOTER_ID, DEFAULT_PRICE_PER_MINUTE));
            _scooterStorage.Add(new Scooter("2", 0.2m));

            Scooter scooter = _scooterService.GetScooterById("2");
            scooter.Should().Be(_scooterStorage[1]);
        }

        [TestMethod]
        public void GetScooterById_NonExistingScooterId_ThrowsNonExistingScooterException()
        {
            Action scooter = () => _scooterService.GetScooterById(DEFAULT_SCOOTER_ID);
            scooter.Should().Throw<NonExistingScooterException>();
        }

        [TestMethod]
        public void GetScooterById_WithEmptyID_ThrowsInvalidIdException()
        {
            Action action = () => _scooterService.GetScooterById("");
            action.Should().Throw<InvalidIdException>();
        }

        [TestMethod]
        public void GetScooters_AddScooterToStorage_ReturnStorageWithScooters()
        {
            _scooterStorage.Add(new Scooter(DEFAULT_SCOOTER_ID, DEFAULT_PRICE_PER_MINUTE));

            var getScooters = _scooterService.GetScooters();

            Scooter firstScooter = getScooters.First();
            firstScooter.Id.Should().Be(DEFAULT_SCOOTER_ID);
            firstScooter.PricePerMinute.Should().Be(DEFAULT_PRICE_PER_MINUTE);
        }

        [TestMethod]
        public void GetScooters_NoScootersInStorage_ThrowEmptyScooterStorageException()
        {
            Action getScooters = () => _scooterService.GetScooters();
            getScooters.Should().Throw<EmptyScooterStorageException>();
        }

        [TestMethod]
        public void RemoveScooter_WithId_ScooterRemoved()
        {
            _scooterStorage.Add(new Scooter(DEFAULT_SCOOTER_ID, DEFAULT_PRICE_PER_MINUTE));
            _scooterService.RemoveScooter(DEFAULT_SCOOTER_ID);

            _scooterStorage.Count.Should().Be(0);
        }

        [TestMethod]
        public void RemoveScooter_WithIdThatDoesNotExist_ThrowsNonExistingScooterException()
        {
            Action scooter = () => _scooterService.RemoveScooter(DEFAULT_SCOOTER_ID);
            scooter.Should().Throw<NonExistingScooterException>();
        }

        [TestMethod]
        public void RemoveScooter_WithEmptyID_ThrowsInvalidIdException()
        {
            Action action = () => _scooterService.RemoveScooter("");
            action.Should().Throw<InvalidIdException>();
        }
    }
}