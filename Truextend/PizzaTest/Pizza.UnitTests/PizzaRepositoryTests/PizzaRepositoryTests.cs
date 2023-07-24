using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Truextend.PizzaTest.Data.Models;
using Truextend.PizzaTest.Data;
using Moq;
using Truextend.PizzaTest.Configuration.Models;

using Truextend.PizzaTest.Data.Repository;
using FluentAssertions;


namespace Truextend.PizzaTest.UnitTests.PizzaRepositoryTests
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
    public class PizzaRepositoryTests
    {
        private static DbContextOptions<TestPizzaDbContext> _options = new DbContextOptionsBuilder<TestPizzaDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        private TestPizzaDbContext _context;
        private List<Pizza> _data;
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

            _data = new List<Pizza>
            {
                new Pizza { Id = Guid.NewGuid(), Name = "Pizza 1", Description = "Desc 1", LargeImageUrl = "url1", SmallImageUrl = "url1" },
                new Pizza { Id = Guid.NewGuid(), Name = "Pizza 2", Description = "Desc 2", LargeImageUrl = "url2", SmallImageUrl = "url2" },
                new Pizza { Id = Guid.NewGuid(), Name = "Pizza 3", Description = "Desc 3", LargeImageUrl = "url3", SmallImageUrl = "url3" }
            };

            _context.Pizza.AddRange(_data);
            _context.SaveChanges();
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllPizzas()
        {
            // Arrange
            var repository = new PizzaRepository(_context);

            // Act
            var result = (await repository.GetAllAsync()).ToList();

            // Assert
            result.Should().BeEquivalentTo(_data);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsCorrectPizza()
        {
            // Arrange
            var pizzaToFind = _data[1];
            var repository = new PizzaRepository(_context);

            // Act
            var result = await repository.GetByIdAsync(pizzaToFind.Id);

            // Assert
            result.Should().BeEquivalentTo(pizzaToFind);
        }
        [Test]
        public async Task GetToppingsForPizzaAsync_ReturnsToppingsForGivenPizzaId()
        {
            // Arrange
            var pizzaToFind = _data[0];
            var toppingsForPizza = new List<Topping>
            {
                new Topping { Id = Guid.NewGuid(), Name = "Topping 1" },
                new Topping { Id = Guid.NewGuid(), Name = "Topping 2" },
                new Topping { Id = Guid.NewGuid(), Name = "Topping 3" }
            };

            _context.Topping.AddRange(toppingsForPizza);
            await _context.SaveChangesAsync();

            pizzaToFind.Toppings = toppingsForPizza;
            _context.Pizza.Update(pizzaToFind);
            await _context.SaveChangesAsync();

            var repository = new PizzaRepository(_context);

            // Act
            var result = await repository.GetToppingsForPizzaAsync(pizzaToFind.Id);

            // Assert
            result.Should().BeEquivalentTo(pizzaToFind.Toppings);
        }

        [Test]
        public async Task AddToppingToPizzaAsync_AddsToppingToPizza()
        {
            // Arrange
            var pizzaToUpdate = _data[1];
            var toppingToAdd = new Topping { Id = Guid.NewGuid(), Name = "Locoto" };
            _context.Topping.Add(toppingToAdd);
            await _context.SaveChangesAsync();
            var repository = new PizzaRepository(_context);

            // Act
            var result = await repository.AddToppingToPizzaAsync(pizzaToUpdate.Id, toppingToAdd.Id);

            // Assert
            result.Toppings.Should().Contain(toppingToAdd);
        }

        [Test]
        public async Task CreateAsync_CreatesNewPizza()
        {
            // Arrange
            var newPizza = new Pizza { Name = "New Pizza", Description = "This is a new pizza.", LargeImageUrl = "url4", SmallImageUrl = "url4" };
            var repository = new PizzaRepository(_context);

            // Act
            var createdPizza = await repository.CreateAsync(newPizza);

            // Assert
            createdPizza.Should().NotBeNull();
            createdPizza.Id.Should().NotBeEmpty();
            createdPizza.Name.Should().Be(newPizza.Name);
            createdPizza.Description.Should().Be(newPizza.Description);

            var pizzaInDb = await _context.Pizza.FindAsync(createdPizza.Id);
            pizzaInDb.Should().NotBeNull();
            pizzaInDb.Name.Should().BeEquivalentTo(newPizza.Name);
        }

        [Test]
        public async Task DeleteAsync_DeletesPizza()
        {
            // Arrange
            var pizzaToDelete = _data[1];
            var repository = new PizzaRepository(_context);

            // Act
            await repository.DeleteAsync(pizzaToDelete);
            await _context.SaveChangesAsync();

            // Assert
            var deletedPizza = await _context.Pizza.FindAsync(pizzaToDelete.Id);
            deletedPizza.Should().BeNull();
        }
        [Test]
        public async Task DeleteToppingFromPizzaAsync_RemovesToppingFromPizza()
        {
            // Arrange
            var pizzaToUpdate = _data[0];
            var toppingsForPizza = new List<Topping>
            {
                new Topping { Id = Guid.NewGuid(), Name = "Topping 1" },
                new Topping { Id = Guid.NewGuid(), Name = "Topping 2" },
                new Topping { Id = Guid.NewGuid(), Name = "Topping 3" }
            };

            _context.Topping.AddRange(toppingsForPizza);
            await _context.SaveChangesAsync();

            pizzaToUpdate.Toppings = toppingsForPizza;
            _context.Pizza.Update(pizzaToUpdate);
            await _context.SaveChangesAsync();

            var repository = new PizzaRepository(_context);

            // Act
            var toppingToRemove = pizzaToUpdate.Toppings.First();
            await repository.DeleteToppingFromPizzaAsync(pizzaToUpdate.Id, toppingToRemove.Id);

            // Assert
            var updatedPizza = await _context.Pizza.Include(p => p.Toppings).FirstOrDefaultAsync(p => p.Id == pizzaToUpdate.Id);
            updatedPizza.Toppings.Should().NotContain(toppingToRemove);
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
