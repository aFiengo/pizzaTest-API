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
using Truextend.PizzaTest.Logic.Managers.Base;
using Truextend.PizzaTest.Logic.Managers.Interface;
using Truextend.PizzaTest.Logic.Models;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<PizzaDTO> GetByIdAsync(Guid id)
        {
            var pizza = await _uow.PizzaRepository.GetByIdAsync(id);
            return _mapper.Map<PizzaDTO>(pizza);
        }
        public async Task<PizzaDTO> CreateAsync(PizzaDTO pizzaDto)
        {
            return null;
        }
        public async Task<PizzaDTO> AddToppingToPizzaAsync(Guid pizzaId, Guid toppingId)
        {
            var pizza = await _uow.PizzaRepository.AddToppingToPizzaAsync(pizzaId, toppingId);
            if (pizza == null)
            {
                throw new NotFoundException($"Pizza with ID {pizzaId} not found.");
            }

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
        public async Task<PizzaDTO> UpdateAsync(Guid id, PizzaDTO pizzaToUpdate)
        {
            var existingPizza = await _uow.PizzaRepository.GetByIdAsync(id);
            if (existingPizza == null)
            {
                throw new NotFoundException($"Pizza with ID {id} not found.");
            }

            var updatedToppings = pizzaToUpdate.Toppings.Select(t => _mapper.Map<Topping>(t)).ToList();

            existingPizza.Name = pizzaToUpdate.Name;
            existingPizza.Toppings = updatedToppings;

            await _uow.PizzaRepository.UpdateAsync(existingPizza);
            await _uow.SaveChangesAsync();

            return _mapper.Map<PizzaDTO>(existingPizza);
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

        public async Task<IEnumerable<PizzaDTO>>GetAllAsync()
        {
            return null;
        }
    }
}
