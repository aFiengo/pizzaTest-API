﻿using AutoMapper;
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
        public async Task<PizzaDTO> CreateAsync(PizzaDTO pizzaToAdd)
        {
            var pizza = _mapper.Map<Pizza>(pizzaToAdd);
            var createdPizza = await _uow.PizzaRepository.CreateAsync(pizza);
            return _mapper.Map<PizzaDTO>(createdPizza);
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
        public async Task<IEnumerable<ToppingDTO>> GetToppingsForPizzaAsync(Guid pizzaId)
        {
            var pizza = await _uow.PizzaRepository.GetByIdAsync(pizzaId);
            if (pizza == null)
            {
                throw new NotFoundException($"Pizza with ID {pizzaId} not found.");
            }
            var toppings = pizza.PizzaToppings
                            .Select(pt => pt.Topping);

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
            existingPizza.PizzaToppings = updatedToppings.Select(t => new PizzaTopping
            {
                PizzaId = existingPizza.Id,
                ToppingId = t.Id
            }).ToList();

            await _uow.PizzaRepository.UpdateAsync(existingPizza);
            await _uow.SaveChangesAsync();

            return _mapper.Map<PizzaDTO>(existingPizza);
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            var existingPizza = await _uow.PizzaRepository.GetByIdAsync(id);
            if (existingPizza == null)
            {
                throw new NotFoundException($"Pizza with ID {id} not found.");
            }

            await _uow.PizzaRepository.DeleteAsync(existingPizza);
            return await _uow.PizzaRepository.GetByIdAsync(id) == null;
        }
    }
}
