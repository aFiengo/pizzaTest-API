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
using FluentAssertions;


namespace PizzaTest.UnitTests.PizzaRepositoryTests
{
    [TestFixture]
    public class PizzaRepositoryTests
    {
        private static DbContextOptions<PizzaDbContext> _options = new DbContextOptionsBuilder<PizzaDbContext>()
        .UseInMemoryDatabase(databaseName: "TestDb")
        .Options;

        private PizzaDbContext _context;
        private List<Pizza> _data;
        private Mock<PizzaDbContext> _mockContext;

        [OneTimeSetUp]
        public void Setup()
        {
            _data = new List<Pizza>
        {
            new Pizza { Id = Guid.NewGuid(), Name = "Pizza 1" },
            new Pizza { Id = Guid.NewGuid(), Name = "Pizza 2" },
            new Pizza { Id = Guid.NewGuid(), Name = "Pizza 3" }
        };

            var mockPizzaSet = _data.ToDbSet();

            _mockContext = new Mock<PizzaDbContext>();

            _mockContext.Setup(c => c.Set<Pizza>()).Returns(mockPizzaSet);

            _context = _mockContext.Object;
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

        [OneTimeTearDown]
        public void CleanUp()
        {
            _mockContext.Object.Database.EnsureDeleted();
        }
    }
    public static class DbSetExtensions
    {
        public static DbSet<T> ToDbSet<T>(this IEnumerable<T> source) where T : class
        {
            var mockDbSet = new Mock<DbSet<T>>();
            var data = source.AsQueryable();

            mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockDbSet.Object;
        }
    }
}
