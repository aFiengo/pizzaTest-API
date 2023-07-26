using AutoMapper;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Truextend.PizzaTest.Configuration.Models;
using Truextend.PizzaTest.Data;
using Truextend.PizzaTest.Data.Exceptions;
using Truextend.PizzaTest.Data.Models;
using Truextend.PizzaTest.Logic.Exceptions;
using Truextend.PizzaTest.Logic.Managers;
using Truextend.PizzaTest.Logic.Managers.Interface;
using Truextend.PizzaTest.Logic.Models;

namespace Truextend.PizzaTest.UnitTests.ManagesTests.ToppingManagerTests
{
    [TestFixture]
    public class ToppingManagerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IMapper> _mockMapper;
        private ToppingManager _toppingManager;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _toppingManager = new ToppingManager(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        [Test]
        public void GetByIdAsync_ToppingDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var nonExistingtId = Guid.NewGuid();
            _mockUnitOfWork.Setup(uow => uow.ToppingRepository.GetByIdAsync(nonExistingtId))
                .Throws(new DatabaseException($"Entity of type Topping with id {nonExistingtId} not found."));

            // Act
            Func<Task> action = async () => await _toppingManager.GetByIdAsync(nonExistingtId);

            // Assert
            action.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Topping with ID {nonExistingtId} not found.");
        }

        [Test]
        public async Task GetByIdAsync_InvalidId_ThrowsNullReferenceException()
        {
            // Arrange
            var invalidId = Guid.Empty;

            // Act
            Func<Task> action = async () => await _toppingManager.GetByIdAsync(invalidId);

            // Assert
            await action.Should().ThrowAsync<NullReferenceException>();
        }

        [Test]
        public async Task CreateAsync_NullTopping_ThrowsNullReferenceException()
        {
            // Arrange
            ToppingDTO toppingToCreate = null;

            // Act
            Func<Task> action = async () => await _toppingManager.CreateAsync(toppingToCreate);

            // Assert
            await action.Should().ThrowAsync<NullReferenceException>();
        }

        [Test]
        public async Task UpdateAsync_ToppingDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();
            var toppingToUpdate = new ToppingDTO();
            _mockUnitOfWork.Setup(uow => uow.ToppingRepository.GetByIdAsync(nonExistentId))
                .Returns(Task.FromResult((Topping)null));

            // Act
            Func<Task> action = async () => await _toppingManager.UpdateAsync(nonExistentId, toppingToUpdate);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Topping with ID {nonExistentId} not found.");
        }

        [Test]
        public async Task UpdateAsync_InvalidId_ThrowsNullReferenceException()
        {
            // Arrange
            var invalidId = Guid.Empty;
            var toppingToUpdate = new ToppingDTO();

            // Act
            Func<Task> action = async () => await _toppingManager.UpdateAsync(invalidId, toppingToUpdate);

            // Assert
            await action.Should().ThrowAsync<NullReferenceException>();
        }

        [Test]
        public async Task DeleteAsync_ToppingDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();
            _mockUnitOfWork.Setup(uow => uow.ToppingRepository.GetByIdAsync(nonExistentId))
                .Returns(Task.FromResult((Topping)null));

            // Act
            Func<Task> action = async () => await _toppingManager.DeleteAsync(nonExistentId);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Topping with ID {nonExistentId} not found.");
        }

        [Test]
        public async Task DeleteAsync_InvalidId_ThrowsArgumentException()
        {
            // Arrange
            var invalidId = Guid.Empty;
            _mockUnitOfWork.Setup(uow => uow.ToppingRepository.GetByIdAsync(invalidId))
                .Returns(Task.FromResult((Topping)null));

            // Act
            Func<Task> action = async () => await _toppingManager.DeleteAsync(invalidId);

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Test]
        public async Task DeleteAsync_ToppingOnMultiplePizzas_ReturnsFalse()
        {
            // Arrange
            var toppingId = Guid.NewGuid();

            var mockTopping = new Topping
            {
                Id = toppingId,
                Pizzas = new List<Pizza> { new Pizza(), new Pizza() }
            };

            _mockUnitOfWork.Setup(uow => uow.ToppingRepository.GetPizzasWithToppingByIdAsync(toppingId))
                .Returns(Task.FromResult(mockTopping));

            // Act
            var result = await _toppingManager.DeleteAsync(toppingId);

            // Assert
            result.Should().BeFalse();
        }
    }
}
