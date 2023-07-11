using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Truextend.PizzaTest.Logic.Exceptions;
using Truextend.PizzaTest.Data;
using Truextend.PizzaTest.Data.Models;
using Truextend.PizzaTest.Logic.Managers;
using Truextend.PizzaTest.Logic.Managers.Interface;
using Truextend.PizzaTest.Logic.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Truextend.PizzaTest.Configuration.Models;

namespace Truextend.PizzaTest.Logic.Managers
{
    public class PizzaManager : IPizzaManager
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly PizzaDefaultSettings _defaultSettings;

        public PizzaManager(IUnitOfWork uow, IMapper mapper, PizzaDefaultSettings defaultSettings)
        {
            _uow = uow;
            _mapper = mapper;
            _defaultSettings = defaultSettings;
        }
        public async Task<IEnumerable<PizzaDTO>> GetAllAsync()
        {
            var pizzas = await _uow.PizzaRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PizzaDTO>>(pizzas);
        }
        public async Task<PizzaDTO> GetByIdAsync(Guid id)
        {
            var pizza = await _uow.PizzaRepository.GetByIdAsync(id);
            return _mapper.Map<PizzaDTO>(pizza);
        }
        public async Task<PizzaDTO> CreateAsync(PizzaDTO pizzaToCreate)
        {
            if (pizzaToCreate == null)
            {
                throw new ArgumentNullException(nameof(pizzaToCreate));
            }
            var pizzaEntity = _mapper.Map<Pizza>(pizzaToCreate);
            if (pizzaEntity == null)
            {
                throw new InvalidOperationException("Failed to map PizzaCreateDTO to Pizza.");
            }

            pizzaEntity.Id= Guid.NewGuid();
            pizzaEntity.SmallImageUrl = _defaultSettings.DefaultSmallImageUrl;
            pizzaEntity.LargeImageUrl = _defaultSettings.DefaultLargeImageUrl;

            var createdPizza = await _uow.PizzaRepository.CreateAsync(pizzaEntity);

            await _uow.SaveChangesAsync();

            var pizzaDto = _mapper.Map<PizzaDTO>(createdPizza);
            return pizzaDto;
        }
        public async Task<Dictionary<string, object>> AddToppingToPizzaAsync(Guid pizzaId, Guid toppingId)
        {
            var pizza = await _uow.PizzaRepository.GetByIdAsync(pizzaId);
            if (pizza == null)
            {
                throw new NotFoundException($"Pizza with ID {pizzaId} not found.");
            }

            var topping = await _uow.ToppingRepository.GetByIdAsync(toppingId);
            if (topping == null)
            {
                throw new NotFoundException($"Topping with ID {toppingId} not found.");
            }

            pizza.Toppings.Add(topping);         

            await _uow.SaveChangesAsync();

            var result = new Dictionary<string, object>
            {
                { "Pizza", _mapper.Map<PizzaDTO>(pizza) },
                { "Topping", _mapper.Map<ToppingDTO>(topping) }
            };

            return result;
        }
        public async Task<IEnumerable<ToppingDTO>> GetToppingsForPizzaAsync(Guid id)
        {
            var pizza = await _uow.PizzaRepository.GetByIdAsync(id);
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Invalid ID.");
            }
            if (pizza == null)
            {
                throw new NotFoundException($"Pizza with ID {id} not found.");
            }
            var toppings = pizza.Toppings;

            return _mapper.Map<IEnumerable<ToppingDTO>>(toppings);
        }
        public async Task<PizzaDTO> UpdateAsync(Guid id, PizzaDTO pizzaToUpdate)
        {
            var existingPizza = await _uow.PizzaRepository.GetByIdAsync(id);
            if (existingPizza == null)
            {
                throw new NotFoundException($"Pizza with ID {id} not found.");
            }

            existingPizza.Name = pizzaToUpdate.Name;

            var result = await _uow.PizzaRepository.UpdateAsync(existingPizza);
            await _uow.SaveChangesAsync();

            return _mapper.Map<PizzaDTO>(result);
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            var pizza = await _uow.PizzaRepository.GetByIdAsync(id);
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Invalid ID.");
            }
            if (pizza == null)
            {
                throw new NotFoundException($"Pizza with ID {id} not found.");
            }

            await _uow.PizzaRepository.DeleteAsync(pizza);
            await _uow.SaveChangesAsync();

            return await _uow.PizzaRepository.GetByIdAsync(id) == null;
        }

        public async Task<bool> DeleteToppingFromPizzaAsync(Guid pizzaId, Guid toppingId)
        {
            var pizza = await _uow.PizzaRepository.GetByIdAsync(pizzaId);
            if (pizza == null)
            {
                throw new NotFoundException($"Pizza with ID {pizzaId} not found.");
            }

            var topping = pizza.Toppings.FirstOrDefault(t => t.Id == toppingId);
            if (topping == null)
            {
                throw new NotFoundException($"Topping with ID {toppingId} not found on Pizza with ID {pizzaId}.");
            }

            pizza.Toppings.Remove(topping);
            await _uow.SaveChangesAsync();

            return true;
        }

    }
}
