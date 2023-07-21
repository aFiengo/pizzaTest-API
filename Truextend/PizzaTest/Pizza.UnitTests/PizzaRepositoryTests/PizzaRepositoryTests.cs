﻿using Microsoft.EntityFrameworkCore;
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
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Truextend.PizzaTest.UnitTests.PizzaRepositoryTests
{
    [TestFixture]
    public class PizzaRepositoryTests
    {
        private static DbContextOptions<PizzaDbContext> _options = new DbContextOptionsBuilder<PizzaDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        private PizzaDbContext _context;
        private List<Pizza> _data;
        private IApplicationConfiguration _appConfiguration;


        [OneTimeSetUp]
        public void Setup()
        {

            var mockAppConfiguration = new Mock<IApplicationConfiguration>();
            mockAppConfiguration.Setup(ac => ac.GetDatabaseConnectionString())
                .Returns(new ConnectionStrings { DATABASE = "TestDb" });

            _appConfiguration = mockAppConfiguration.Object; // Use this line to set _appConfiguration

            _context = new PizzaDbContext(_appConfiguration); // Pass _appConfiguration here
            _context.Database.EnsureCreated();

            _data = new List<Pizza>
        {
            new Pizza { Id = Guid.NewGuid(), Name = "Pizza 1" },
            new Pizza { Id = Guid.NewGuid(), Name = "Pizza 2" },
            new Pizza { Id = Guid.NewGuid(), Name = "Pizza 3" }
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

        [OneTimeTearDown]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
        }
    }

    internal class ConnectionStrings : DatabaseConnectionString
    {
        public string DATABASE { get; set; }
    }
}
