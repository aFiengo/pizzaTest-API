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
using Truextend.PizzaTest.Logic.Models;

namespace PizzaTest.UnitTests.PizzaManagerTests
{
    [TestFixture]
    public class PizzaManagerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IMapper> _mockMapper;
        private PizzaManager _pizzaManager;
        private Mock<PizzaDefaultSettings> _mockDefaultSettings;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockDefaultSettings = new Mock<PizzaDefaultSettings>();
            _pizzaManager = new PizzaManager(_mockUnitOfWork.Object, _mockMapper.Object, _mockDefaultSettings.Object);
        }

        [Test]
        public void GetByIdAsync_PizzaDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var nonExistingtId = Guid.NewGuid();
            _mockUnitOfWork.Setup(uow => uow.PizzaRepository.GetByIdAsync(nonExistingtId))
                .Throws(new DatabaseException($"Entity of type Pizza with id {nonExistingtId} not found."));

            // Act
            Func<Task> action = async () => await _pizzaManager.GetByIdAsync(nonExistingtId);

            // Assert
            action.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Pizza with ID {nonExistingtId} not found.");
        }

        [Test]
        public async Task GetByIdAsync_InvalidId_ThrowsNullReferenceException()
        {
            // Arrange
            var invalidId = Guid.Empty;

            // Act
            Func<Task> action = async () => await _pizzaManager.GetByIdAsync(invalidId);

            // Assert
            await action.Should().ThrowAsync<NullReferenceException>();
        }

        [Test]
        public async Task CreateAsync_NullPizza_ThrowsArgumentNullException()
        {
            // Arrange
            PizzaDTO pizzaToCreate = null;

            // Act
            Func<Task> action = async () => await _pizzaManager.CreateAsync(pizzaToCreate);

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Test]
        public async Task UpdateAsync_PizzaDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();
            var pizzaToUpdate = new PizzaDTO();
            _mockUnitOfWork.Setup(uow => uow.PizzaRepository.GetByIdAsync(nonExistentId))
                .Returns(Task.FromResult((Pizza)null));

            // Act
            Func<Task> action = async () => await _pizzaManager.UpdateAsync(nonExistentId, pizzaToUpdate);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Pizza with ID {nonExistentId} not found.");
        }

        [Test]
        public async Task UpdateAsync_InvalidId_ThrowsNullReferenceException()
        {
            // Arrange
            var invalidId = Guid.Empty;
            var pizzaToUpdate = new PizzaDTO();

            // Act
            Func<Task> action = async () => await _pizzaManager.UpdateAsync(invalidId, pizzaToUpdate);

            // Assert
            await action.Should().ThrowAsync<NullReferenceException>();
        }

        [Test]
        public async Task DeleteAsync_PizzaDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();
            _mockUnitOfWork.Setup(uow => uow.PizzaRepository.GetByIdAsync(nonExistentId))
                .Returns(Task.FromResult((Pizza)null));

            // Act
            Func<Task> action = async () => await _pizzaManager.DeleteAsync(nonExistentId);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Pizza with ID {nonExistentId} not found.");
        }

        [Test]
        public async Task DeleteAsync_InvalidId_ThrowsArgumentException()
        {
            // Arrange
            var invalidId = Guid.Empty;
            _mockUnitOfWork.Setup(uow => uow.PizzaRepository.GetByIdAsync(invalidId))
                .Returns(Task.FromResult((Pizza)null));

            // Act
            Func<Task> action = async () => await _pizzaManager.DeleteAsync(invalidId);

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Test]
        public async Task AddToppingToPizzaAsync_InvalidPizzaId_ThrowsNotFoundException()
        {
            // Arrange
            var invalidPizzaId = Guid.NewGuid();
            var validToppingId = Guid.NewGuid();
            _mockUnitOfWork.Setup(uow => uow.PizzaRepository.GetByIdAsync(invalidPizzaId))
                .Returns(Task.FromResult((Pizza)null));

            // Act
            Func<Task> action = async () => await _pizzaManager.AddToppingToPizzaAsync(invalidPizzaId, validToppingId);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Pizza with ID {invalidPizzaId} not found.");
        }

        [Test]
        public async Task AddToppingToPizzaAsync_InvalidToppingId_ThrowsNotFoundException()
        {
            // Arrange
            var validPizzaId = Guid.NewGuid();
            var invalidToppingId = Guid.NewGuid();
            var mockPizza = new Mock<Pizza>();
            _mockUnitOfWork.Setup(uow => uow.PizzaRepository.GetByIdAsync(validPizzaId))
                .Returns(Task.FromResult(mockPizza.Object));
            _mockUnitOfWork.Setup(uow => uow.ToppingRepository.GetByIdAsync(invalidToppingId))
                .Returns(Task.FromResult((Topping)null));

            // Act
            Func<Task> action = async () => await _pizzaManager.AddToppingToPizzaAsync(validPizzaId, invalidToppingId);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Topping with ID {invalidToppingId} not found.");
        }

        [Test]
        public async Task GetToppingsForPizzaAsync_InvalidPizzaId_ThrowsNotFoundException()
        {
            // Arrange
            var invalidPizzaId = Guid.NewGuid();
            _mockUnitOfWork.Setup(uow => uow.PizzaRepository.GetByIdAsync(invalidPizzaId))
                .Returns(Task.FromResult((Pizza)null));

            // Act
            Func<Task> action = async () => await _pizzaManager.GetToppingsForPizzaAsync(invalidPizzaId);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Pizza with ID {invalidPizzaId} not found.");
        }

        [Test]
        public async Task GetToppingsForPizzaAsync_EmptyId_ThrowsArgumentException()
        {
            // Arrange
            var invalidPizzaId = Guid.Empty;

            // Act
            Func<Task> action = async () => await _pizzaManager.GetToppingsForPizzaAsync(invalidPizzaId);

            // Assert
            await action.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Invalid ID.");
        }

        [Test]
        public async Task DeleteToppingFromPizzaAsync_InvalidPizzaId_ThrowsNotFoundException()
        {
            // Arrange
            var invalidPizzaId = Guid.NewGuid();
            var validToppingId = Guid.NewGuid();
            _mockUnitOfWork.Setup(uow => uow.PizzaRepository.GetByIdAsync(invalidPizzaId))
                .Returns(Task.FromResult((Pizza)null));

            // Act
            Func<Task> action = async () => await _pizzaManager.DeleteToppingFromPizzaAsync(invalidPizzaId, validToppingId);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Pizza with ID {invalidPizzaId} not found.");
        }

        [Test]
        public async Task DeleteToppingFromPizzaAsync_InvalidToppingId_ThrowsNotFoundException()
        {
            // Arrange
            var validPizzaId = Guid.NewGuid();
            var invalidToppingId = Guid.NewGuid();
            var mockPizza = new Mock<Pizza>();
            mockPizza.Object.Toppings = new List<Topping>();
            _mockUnitOfWork.Setup(uow => uow.PizzaRepository.GetByIdAsync(validPizzaId))
                .Returns(Task.FromResult(mockPizza.Object));

            // Act
            Func<Task> action = async () => await _pizzaManager.DeleteToppingFromPizzaAsync(validPizzaId, invalidToppingId);

            // Assert
            await action.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Topping with ID {invalidToppingId} not found on Pizza with ID {validPizzaId}.");
        }
    }
}
