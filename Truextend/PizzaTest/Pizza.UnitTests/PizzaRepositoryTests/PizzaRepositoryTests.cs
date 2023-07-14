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
using System.Xml.Linq;
using Truextend.PizzaTest.Data.Repository;

namespace PizzaTest.UnitTests.PizzaRepositoryTests
{
    [TestFixture]
    public class PizzaRepositoryTests
    {
        private DbContextOptions<PizzaDbContext> _options;
        private List<Pizza> _data;
        private Mock<IApplicationConfiguration> _mockConfig;

        [SetUp]
        public void Setup()
        {
            var databaseConnectionString = new DatabaseConnectionString { DATABASE = "DataSource=:memory:" };

            _mockConfig = new Mock<IApplicationConfiguration>();
            _mockConfig.Setup(config => config.GetDatabaseConnectionString()).Returns(databaseConnectionString);

            _options = new DbContextOptionsBuilder<PizzaDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _data = new List<Pizza>
        {
            new Pizza { Id = Guid.NewGuid(), Name = "Pizza 1" },
            new Pizza { Id = Guid.NewGuid(), Name = "Pizza 2" },
            new Pizza { Id = Guid.NewGuid(), Name = "Pizza 3" }
        };

            using (var context = new PizzaDbContext(_options))
            {
                context.Pizza.AddRange(_data);
                context.SaveChanges();
            }
        }
        [Test]
        public async Task GetAllAsync_ReturnsAllPizzas()
        {
            // Arrange
            PizzaRepository pizzaRepository;
            using (var context = new PizzaDbContext(_options))
            {
                pizzaRepository = new PizzaRepository(context);
            }

            // Act
            var result = await pizzaRepository.GetAllAsync();

            // Assert
            Assert.That(result, Is.EqualTo(_data));
        }

        [Test]
        public async Task GetByIdAsync_ReturnsCorrectPizza()
        {
            // Arrange
            var targetPizza = _data[0];
            PizzaRepository pizzaRepository;
            using (var context = new PizzaDbContext(_options))
            {
                pizzaRepository = new PizzaRepository(context);
            }

            // Act
            var result = await pizzaRepository.GetByIdAsync(targetPizza.Id);

            // Assert
            Assert.That(result, Is.EqualTo(targetPizza));
        }
    }
}
