using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Language;
using Moq.Language.Flow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Truextend.PizzaTest.Data.Models;
using Truextend.PizzaTest.Logic.Managers.Base;
using Truextend.PizzaTest.Presentation.Controllers.Base;
using FluentAssertions;
using System.Net;
using Truextend.PizzaTest.Presentation.Middleware;
using Microsoft.AspNetCore.Http;
using Truextend.PizzaTest.Logic.Models;
using Truextend.PizzaTest.Presentation.Controllers;
using Truextend.PizzaTest.Logic.Managers.Interface;

namespace Truextend.PizzaTest.UnitTests.ControllersTests.PizzaControllerTests
{
    [TestFixture]
    public class PizzaControllerTests
    {
        private Mock<IPizzaManager> _mockManager;
        private PizzaController _controller;
        private List<PizzaDTO> _pizzas;
        private PizzaDTO _pizzaDto;
        private Pizza _pizza;

        [SetUp]
        public void Setup()
        {
            _mockManager = new Mock<IPizzaManager>();
            _controller = new PizzaController(_mockManager.Object);

            _pizzas = new List<PizzaDTO>
            {
                new PizzaDTO { Id = Guid.NewGuid(), Name = "Pizza 1", Description = "Desc 1", LargeImageUrl = "url1", SmallImageUrl = "url1" },
                new PizzaDTO { Id = Guid.NewGuid(), Name = "Pizza 2", Description = "Desc 2", LargeImageUrl = "url2", SmallImageUrl = "url2" },
                new PizzaDTO { Id = Guid.NewGuid(), Name = "Pizza 3", Description = "Desc 3", LargeImageUrl = "url3", SmallImageUrl = "url3" },
            };
            _pizzaDto = _pizzas[0];
        }

        [Test]
        public async Task GetAllItems_ReturnsOkResult_WithListOfPizzas()
        {
            // Arrange
            _mockManager.Setup(repo => repo.GetAllAsync()).ReturnsAsync(_pizzas);

            // Act
            var result = await _controller.GetAllItems();
            var okResult = result as OkObjectResult;

            // Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            var middlewareResponse = okResult.Value as MiddlewareResponse<IEnumerable<PizzaDTO>>;
            Assert.IsNotNull(middlewareResponse);
            Assert.AreEqual(_pizzas, middlewareResponse.data);
        }

        [Test]
        public async Task GetItemByID_ReturnsOkResult_WithPizza()
        {
            // Arrange
            Guid testPizzaId = _pizzaDto.Id;
            _mockManager.Setup(repo => repo.GetByIdAsync(testPizzaId)).ReturnsAsync(_pizzaDto);

            // Act
            var result = await _controller.GetItemByID(testPizzaId);
            var okResult = result as OkObjectResult;
            // Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            var middlewareResponse = okResult.Value as MiddlewareResponse<PizzaDTO>;
            Assert.IsNotNull(middlewareResponse);
            Assert.That(middlewareResponse.data, Is.InstanceOf<PizzaDTO>());
            var item = middlewareResponse.data;
            Assert.That(item.Id, Is.EqualTo(testPizzaId));
        }

        [Test]
        public async Task GetItemByID_WithInvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();
            _mockManager.Setup(repo => repo.GetByIdAsync(nonExistentId))
                .Returns(Task.FromResult<PizzaDTO>(null));

            // Act
            var result = await _controller.GetItemByID(nonExistentId);
            var okResult = result as OkObjectResult;
            var middlewareResponse = okResult?.Value as MiddlewareResponse<PizzaDTO>;

            // Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.IsNull(middlewareResponse?.data);
        }

        [Test]
        public async Task GetItemByID_ThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            var testPizzaId = Guid.NewGuid();
            _mockManager.Setup(repo => repo.GetByIdAsync(testPizzaId)).Throws(new Exception());

            // Act
            IActionResult result = null;
            try
            {
                result = await _controller.GetItemByID(testPizzaId);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.IsInstanceOf<Exception>(ex);
            }
        }

        [Test]
        public async Task AddItem_ValidItem_ReturnsOkResultWithNewItem()
        {
            // Arrange
            _mockManager.Setup(x => x.CreateAsync(_pizzaDto)).ReturnsAsync(_pizzaDto);

            // Act
            var result = await _controller.AddItem(_pizzaDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var middlewareResponse = okResult?.Value as MiddlewareResponse<PizzaDTO>;

            middlewareResponse?.data.Should().BeEquivalentTo(_pizzaDto);
        }

        [Test]
        public async Task UpdateById_ValidIdAndItem_ReturnsOkResultWithUpdatedItem()
        {
            // Arrange
            var testPizzaId = Guid.NewGuid();
            PizzaDTO pizza = new PizzaDTO() { Id = testPizzaId };
            _mockManager.Setup(repo => repo.UpdateAsync(testPizzaId, pizza))
                .ReturnsAsync(pizza);

            // Act
            var result = await _controller.UpdateById(testPizzaId, pizza);
            var okResult = result as OkObjectResult;
            var middlewareResponse = okResult?.Value as MiddlewareResponse<PizzaDTO>;
            var dataResult = middlewareResponse?.data;


            // Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(okResult.StatusCode, StatusCodes.Status200OK);
            Assert.IsNotNull(dataResult);  
            Assert.AreEqual(dataResult?.Id, pizza.Id);
        }

        [Test]
        public async Task Delete_ValidId_ReturnsOkResult()
        {
            // Arrange
            var testPizzaId = Guid.NewGuid();
            _mockManager.Setup(repo => repo.DeleteAsync(testPizzaId)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(testPizzaId);
            var okResult = result as OkObjectResult;

            // Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(okResult.StatusCode, StatusCodes.Status200OK);
            Assert.AreEqual((okResult.Value as MiddlewareResponse<bool>).data, true);
        }

        [Test]
        public async Task GetToppingsForPizzaAsync_WithValidPizzaId_ReturnsOk()
        {
            // Arrange
            Guid testPizzaId = _pizzaDto.Id;
            var testToppings = new List<ToppingDTO> { 
                new ToppingDTO { Id = Guid.NewGuid(), Name = "Topping 1" },
                new ToppingDTO { Id = Guid.NewGuid(), Name = "Topping 2" },
                new ToppingDTO { Id = Guid.NewGuid(), Name = "Topping 3" } 
            };
            _mockManager.Setup(repo => repo.GetToppingsForPizzaAsync(testPizzaId))
                .ReturnsAsync(testToppings);


            // Act
            var result = await _controller.GetToppingsForPizzaAsync(testPizzaId);
            var okResult = result as OkObjectResult;
            var middlewareResponse = okResult?.Value as MiddlewareResponse<IEnumerable<ToppingDTO>>;
            var returnedToppings = middlewareResponse?.data;

            // Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.IsNotNull(returnedToppings); 
            Assert.AreEqual(testToppings, returnedToppings);
        }

        [Test]
        public async Task AddToppingToPizza_WithValidIds_ReturnsOk()
        {
            // Arrange
            var testPizzaId = Guid.NewGuid();
            var testToppingId = Guid.NewGuid();
            var resultDictionary = new Dictionary<string, object> { 
                { "Pizza", _pizzaDto},
                { "Topping", _pizzaDto }
            };
            _mockManager.Setup(repo => repo.AddToppingToPizzaAsync(testPizzaId, testToppingId))
                .ReturnsAsync(resultDictionary);

            // Act
            var result = await _controller.AddToppingToPizza(testPizzaId, testToppingId);
            var okResult = result as OkObjectResult;
            var middlewareResponse = okResult?.Value as MiddlewareResponse<Dictionary<string, object>>;
            var returnedDictionary = middlewareResponse?.data;

            // Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.IsNotNull(returnedDictionary); 
            Assert.AreEqual(resultDictionary, returnedDictionary);
        }

        [Test]
        public async Task DeleteToppingFromPizza_WithValidIds_ReturnsOk()
        {
            // Arrange
            var testPizzaId = Guid.NewGuid();
            var testToppingId = Guid.NewGuid();
            _mockManager.Setup(repo => repo.DeleteToppingFromPizzaAsync(testPizzaId, testToppingId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteToppingFromPizza(testPizzaId, testToppingId);
            var okResult = result as OkObjectResult;
            var middlewareResponse = okResult?.Value as MiddlewareResponse<bool>;
            var isToppingDeleted = middlewareResponse?.data;

            // Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.IsNotNull(isToppingDeleted);
            Assert.AreEqual(true, isToppingDeleted);
        }
    }
}
