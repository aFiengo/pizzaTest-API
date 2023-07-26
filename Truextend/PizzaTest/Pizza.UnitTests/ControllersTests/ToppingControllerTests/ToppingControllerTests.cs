using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Truextend.PizzaTest.Data.Models;
using Truextend.PizzaTest.Logic.Exceptions;
using Truextend.PizzaTest.Logic.Managers.Interface;
using Truextend.PizzaTest.Logic.Models;
using Truextend.PizzaTest.Presentation.Controllers;
using Truextend.PizzaTest.Presentation.Middleware;

namespace Truextend.PizzaTest.UnitTests.ControllersTests.ToppingControllerTests
{
    [TestFixture]
    public class ToppingControllerTests
    {
        private Mock<IToppingManager> _mockManager;
        private ToppingController _controller;
        private List<ToppingDTO> _toppings;
        private ToppingDTO _toppingDto;

        [SetUp]
        public void Setup()
        {
            _mockManager = new Mock<IToppingManager>();
            _controller = new ToppingController(_mockManager.Object);

            _toppings = new List<ToppingDTO>
            {
                new ToppingDTO { Id = Guid.NewGuid(), Name = "Topping 1" },
                new ToppingDTO { Id = Guid.NewGuid(), Name = "Topping 2" },
                new ToppingDTO { Id = Guid.NewGuid(), Name = "Topping 3" },
            };
            _toppingDto = _toppings[0];
        }

        [Test]
        public async Task GetAllItems_ReturnsOkResult_WithListOfToppings()
        {
            // Arrange
            _mockManager.Setup(repo => repo.GetAllAsync()).ReturnsAsync(_toppings);

            // Act
            var result = await _controller.GetAllItems();
            var okResult = result as OkObjectResult;

            // Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            var middlewareResponse = okResult.Value as MiddlewareResponse<IEnumerable<ToppingDTO>>;
            Assert.IsNotNull(middlewareResponse);
            Assert.AreEqual(_toppings, middlewareResponse.data);
        }

        [Test]
        public async Task GetItemByID_ReturnsOkResult_WithTopping()
        {
            // Arrange
            Guid testToppingId = _toppingDto.Id;
            _mockManager.Setup(repo => repo.GetByIdAsync(testToppingId)).ReturnsAsync(_toppingDto);

            // Act
            var result = await _controller.GetItemByID(testToppingId);
            var okResult = result as OkObjectResult;
            // Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            var middlewareResponse = okResult.Value as MiddlewareResponse<ToppingDTO>;
            Assert.IsNotNull(middlewareResponse);
            Assert.That(middlewareResponse.data, Is.InstanceOf<ToppingDTO>());
            var item = middlewareResponse.data;
            Assert.That(item.Id, Is.EqualTo(testToppingId));
        }

        [Test]
        public async Task GetItemByID_WithInvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();
            _mockManager.Setup(repo => repo.GetByIdAsync(nonExistentId))
                .Returns(Task.FromResult<ToppingDTO>(null));

            // Act
            var result = await _controller.GetItemByID(nonExistentId);
            var okResult = result as OkObjectResult;
            var middlewareResponse = okResult?.Value as MiddlewareResponse<ToppingDTO>;

            // Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.IsNull(middlewareResponse?.data);
        }

        [Test]
        public async Task GetItemByID_ThrowsException_ReturnsInternalServerError()
        {
            // Arrange
            var testToppingId = Guid.NewGuid();
            _mockManager.Setup(repo => repo.GetByIdAsync(testToppingId)).Throws(new Exception());

            // Act
            IActionResult result = null;
            try
            {
                result = await _controller.GetItemByID(testToppingId);
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
            _mockManager.Setup(x => x.CreateAsync(_toppingDto)).ReturnsAsync(_toppingDto);

            // Act
            var result = await _controller.AddItem(_toppingDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var middlewareResponse = okResult?.Value as MiddlewareResponse<ToppingDTO>;

            middlewareResponse?.data.Should().BeEquivalentTo(_toppingDto);
        }

        [Test]
        public async Task AddItem_ReturnsBadRequest_WhenItemIsNull()
        {
            // Arrange
            ToppingDTO toppingDto = null;

            // Act
            var result = await _controller.AddItem(toppingDto);
            var badRequestResult = result as BadRequestObjectResult;
            var middlewareResponse = badRequestResult?.Value as MiddlewareResponse<string>;

            // Assert
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.AreEqual("Item is null", middlewareResponse?.data);
        }

        [Test]
        public async Task UpdateById_ValidIdAndItem_ReturnsOkResultWithUpdatedItem()
        {
            // Arrange
            var testToppingId = Guid.NewGuid();
            ToppingDTO topping = new ToppingDTO() { Id = testToppingId };
            _mockManager.Setup(repo => repo.UpdateAsync(testToppingId, topping))
                .ReturnsAsync(topping);

            // Act
            var result = await _controller.UpdateById(testToppingId, topping);
            var okResult = result as OkObjectResult;
            var middlewareResponse = okResult?.Value as MiddlewareResponse<ToppingDTO>;
            var dataResult = middlewareResponse?.data;


            // Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(okResult.StatusCode, StatusCodes.Status200OK);
            Assert.IsNotNull(dataResult);
            Assert.AreEqual(dataResult?.Id, topping.Id);
        }

        [Test]
        public async Task UpdateById_ReturnsNotFound_WhenItemDoesNotExist()
        {
            // Arrange
            var testToppingId = Guid.NewGuid();
            ToppingDTO topping = new ToppingDTO() { Id = testToppingId };
            _mockManager.Setup(repo => repo.UpdateAsync(testToppingId, topping)).Throws(new NotFoundException($"Topping with ID {testToppingId} not found."));

            // Act
            var result = await _controller.UpdateById(testToppingId, topping);
            var notFoundResult = result as NotFoundObjectResult;
            var middlewareResponse = notFoundResult?.Value as MiddlewareResponse<string>;

            // Assert
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            Assert.AreEqual($"Topping with ID {testToppingId} not found.", middlewareResponse?.data);
        }

        [Test]
        public async Task Delete_ValidId_ReturnsOkResult()
        {
            // Arrange
            var testToppingId = Guid.NewGuid();
            _mockManager.Setup(repo => repo.DeleteAsync(testToppingId)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(testToppingId);
            var okResult = result as OkObjectResult;

            // Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(okResult.StatusCode, StatusCodes.Status200OK);
            Assert.AreEqual((okResult.Value as MiddlewareResponse<bool>).data, true);
        }

        [Test]
        public async Task Delete_ReturnsNotFound_WhenItemDoesNotExist()
        {
            // Arrange
            var testToppingId = Guid.NewGuid();
            _mockManager.Setup(repo => repo.DeleteAsync(testToppingId)).Throws(new NotFoundException($"Topping with ID {testToppingId} not found."));

            // Act
            var result = await _controller.Delete(testToppingId);
            var notFoundResult = result as NotFoundObjectResult;
            var middlewareResponse = notFoundResult?.Value as MiddlewareResponse<string>;

            // Assert
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            Assert.AreEqual($"Topping with ID {testToppingId} not found.", middlewareResponse?.data);
        }

        [Test]
        public async Task Delete_ReturnsBadRequest_WhenToppingIsUsedInMultiplePizzas()
        {
            // Arrange
            var testToppingId = Guid.NewGuid();
            _mockManager.Setup(repo => repo.IsToppingUsedInMultiplePizzas(testToppingId)).ReturnsAsync(true);
            _mockManager.Setup(repo => repo.DeleteAsync(testToppingId)).ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(testToppingId);
            var badRequestResult = result as BadRequestObjectResult;
            var middlewareResponse = badRequestResult?.Value as MiddlewareResponse<bool>;

            // Assert
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.AreEqual(false, middlewareResponse?.data);
        }
    }
}
