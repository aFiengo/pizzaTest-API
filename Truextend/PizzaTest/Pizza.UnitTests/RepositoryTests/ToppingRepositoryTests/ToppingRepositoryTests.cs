using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Truextend.PizzaTest.Data.Models;
using Moq;
using Truextend.PizzaTest.Configuration.Models;
using System.Xml.Linq;
using Truextend.PizzaTest.Data.Repository;
using FluentAssertions;

using Truextend.PizzaTest.Data;

namespace Truextend.PizzaTest.UnitTests.RepositoryTests.ToppingRepositoryTests
{
    [TestFixture]
    public class TestPizzaDbContext : PizzaDbContext
    {
        public TestPizzaDbContext(IApplicationConfiguration applicationConfiguration)
        : base(applicationConfiguration)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("TestDb");
        }
    }
    public class ToppingRepositoryTests
    {
        private static DbContextOptions<TestPizzaDbContext> _options = new DbContextOptionsBuilder<TestPizzaDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        private TestPizzaDbContext _context;
        private List<Topping> _data;
        private IApplicationConfiguration _appConfiguration;


        [SetUp]
        public void Setup()
        {

            var mockAppConfiguration = new Mock<IApplicationConfiguration>();
            mockAppConfiguration.Setup(ac => ac.GetDatabaseConnectionString())
                .Returns(new ConnectionStrings { DATABASE = "TestDb" });

            _appConfiguration = mockAppConfiguration.Object;

            _context = new TestPizzaDbContext(_appConfiguration);
            _context.Database.EnsureCreated();

            _data = new List<Topping>
            {
                new Topping { Id = Guid.NewGuid(), Name = "Topping 1" },
                new Topping { Id = Guid.NewGuid(), Name = "Topping 2" },
                new Topping { Id = Guid.NewGuid(), Name = "Topping 3" }
            };

            _context.Topping.AddRange(_data);
            _context.SaveChanges();
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllToppings()
        {
            // Arrange
            var repository = new ToppingRepository(_context);

            // Act
            var result = (await repository.GetAllAsync()).ToList();

            // Assert
            result.Should().BeEquivalentTo(_data);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsCorrectTopping()
        {
            // Arrange
            var toppingToFind = _data[1];
            var repository = new ToppingRepository(_context);

            // Act
            var result = await repository.GetByIdAsync(toppingToFind.Id);

            // Assert
            result.Should().BeEquivalentTo(toppingToFind);
        }

        [Test]
        public async Task CreateAsync_CreatesNewTopping()
        {
            // Arrange
            var newTopping = new Topping { Name = "New Topping" };
            var repository = new ToppingRepository(_context);

            // Act
            var createdTopping = await repository.CreateAsync(newTopping);

            // Assert
            createdTopping.Should().NotBeNull();
            createdTopping.Id.Should().NotBeEmpty();
            createdTopping.Name.Should().Be(newTopping.Name);

            var toppingInDb = await _context.Topping.FindAsync(createdTopping.Id);
            toppingInDb.Should().NotBeNull();
            toppingInDb.Name.Should().BeEquivalentTo(newTopping.Name);
        }

        [Test]
        public async Task DeleteAsync_DeletesTopping()
        {
            // Arrange
            var toppingToDelete = _data[1];
            var repository = new ToppingRepository(_context);

            // Act
            await repository.DeleteAsync(toppingToDelete);
            await _context.SaveChangesAsync();

            // Assert
            var deletedTopping = await _context.Topping.FindAsync(toppingToDelete.Id);
            deletedTopping.Should().BeNull();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }

    internal class ConnectionStrings : DatabaseConnectionString
    {
        public string DATABASE { get; set; }
    }
}
