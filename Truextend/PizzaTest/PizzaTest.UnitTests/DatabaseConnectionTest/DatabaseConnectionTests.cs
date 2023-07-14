using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Truextend.PizzaTest.Data.Scripts;

namespace Truextend.PizzaTest.UnitTests.DbConnectionTest
{
    [TestFixture]
    public class DatabaseConnectionTests
    {
        private Mock<DbContext> _mockDbContext;

        [SetUp]
        public void Setup()
        {
            _mockDbContext = new Mock<DbContext>();
        }

        [Test]
        public void TryOpenConnection_ConnectionOpens_ReturnsTrue()
        {
            var databaseConnectionTest = new DatabaseConnection(_mockDbContext.Object);

            var result = databaseConnectionTest.TryOpenConnection();

            Assert.IsTrue(result);
        }
    }
}
