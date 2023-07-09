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

namespace Truextend.PizzaTest.Logic.Managers
{
    public class PizzaManager : IPizzaManager
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public PizzaManager(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<IEnumerable<PizzaNameDTO>> GetAllPizzaAsync()
        {
            var pizzas = await _uow.PizzaRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PizzaNameDTO>>(pizzas);
        }
        public async Task<PizzaNameDTO> GetPizzaByIdAsync(Guid id)
        {
            var pizza = await _uow.PizzaRepository.GetByIdAsync(id);
            return _mapper.Map<PizzaNameDTO>(pizza);
        }
        public async Task<PizzaDTO> GetPizzaToppingsByIdAAsync(Guid id)
        {
            var pizza = await _uow.PizzaRepository.GetByIdAsync(id);
            return _mapper.Map<PizzaDTO>(pizza);
        }
    public async Task<PizzaDTO> CreatePizzaAsync(PizzaCreateDTO pizzaToCreate)
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

            pizzaEntity.SmallImageUrl = "https://drive.google.com/file/d/1KgTq5_fJ59wrEGi2DwfgSAwDtrot_won/view?usp=drive_link";
            pizzaEntity.LargeImageUrl = "https://drive.google.com/file/d/1nAPT-7X3TPg3Y9cXEHE-CB4Amr4IhnZQ/view?usp=drive_link";

            var createdPizza = await _uow.PizzaRepository.CreatePizzaWithToppingsAsync(pizzaEntity, pizzaToCreate.ToppingIds);

            var pizzaDto = _mapper.Map<PizzaDTO>(createdPizza);
            return pizzaDto;
        }
        public async Task<PizzaDTO> AddToppingToPizzaAsync(Guid pizzaId, Guid toppingId)
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

            await _uow.PizzaRepository.UpdateAsync(pizza);
            await _uow.SaveChangesAsync();

            return _mapper.Map<PizzaDTO>(pizza);
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
        public async Task<PizzaDTO> UpdatePizzaAsync(Guid id, PizzaCreateDTO pizzaToUpdate)
        {
            var existingPizza = await _uow.PizzaRepository.GetByIdAsync(id);
            if (existingPizza == null)
            {
                throw new NotFoundException($"Pizza with ID {id} not found.");
            }

            var updatedToppings = new List<Topping>();
            foreach (var toppingId in pizzaToUpdate.ToppingIds)
            {
                var topping = await _uow.ToppingRepository.GetByIdAsync(toppingId);
                if (topping == null)
                {
                    throw new NotFoundException($"Topping with ID {toppingId} not found.");
                }
                updatedToppings.Add(topping);
            }

            existingPizza.Name = pizzaToUpdate.Name;
            existingPizza.Toppings = updatedToppings;

            var result = await _uow.PizzaRepository.UpdateAsync(existingPizza);
            await _uow.SaveChangesAsync();

            return _mapper.Map<PizzaDTO>(result);
        }
        public async Task<PizzaDTO> DeletePizzaAsync(Guid id)
        {
            var pizza = await _uow.PizzaRepository.GetPizzaWithToppingsAsync(id);
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Invalid ID.");
            }
            if (pizza == null)
            {
                throw new NotFoundException($"Pizza with ID {id} not found.");
            }

            pizza.Toppings.Clear();
            await _uow.SaveChangesAsync();

            _uow.PizzaRepository.Delete(pizza);
            await _uow.SaveChangesAsync();

            return _mapper.Map<PizzaDTO>(pizza);
        }

        public async Task<PizzaDTO> DeleteToppingFromPizzaAsync(Guid pizzaId, Guid toppingId)
        {
            var pizza = await _uow.PizzaRepository.GetPizzaWithToppingsAsync(pizzaId);

            var topping = await _uow.PizzaRepository.DeleteToppingFromPizzaAsync(pizzaId, toppingId);
            if (topping == null)
            {
                throw new NotFoundException($"Topping with ID {toppingId} not found on Pizza with ID {pizzaId}.");
            }

            return _mapper.Map<PizzaDTO>(pizza);
        }

    }
}
