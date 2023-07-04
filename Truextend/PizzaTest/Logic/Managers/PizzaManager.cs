using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Truextend.PizzaTest.Core.Exceptions;
using Truextend.PizzaTest.Data;
using Truextend.PizzaTest.Data.Models;
using Truextend.PizzaTest.Logic.Managers.Base;

namespace Truextend.PizzaTest.Logic.Managers
{
    public class PizzaManager : IGenericManager<Pizza>
    {
        private readonly IUnitOfWork _uow;

        public PizzaManager(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<IEnumerable<Pizza>> GetAllAsync()
        {
            try
            {
                IEnumerable<Pizza> pizzas = await _uow.PizzaRepository.GetAllAsync();
                return await _uow.PizzaRepository.GetAllAsync();
            } catch(Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public async Task<Pizza> GetByIdAsync(Guid id)
        {
            return await _uow.PizzaRepository.GetByIdAsync(id);
        }
        public async Task<Pizza> CreateAsync(Pizza pizzaToAdd)
        {
            if (String.IsNullOrEmpty(pizzaToAdd.Name))
            {
                throw new Exception("A name is required");
            }

            return await _uow.PizzaRepository.CreateAsync(pizzaToAdd);
        }
        public async Task<Pizza> AddToppingToPizzaAsync(Guid pizzaId, Guid toppingId)
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

            var pizzaTopping = new PizzaTopping { PizzaId = pizzaId, ToppingId = toppingId };

            await _uow.PizzaRepository.UpdateAsync(pizza);
            return pizza;
        }
        public async Task<IEnumerable<Topping>> GetToppingsForPizzaAsync(Guid pizzaId)
        {
            var pizza = await _uow.PizzaRepository.GetByIdAsync(pizzaId);
            if (pizza == null)
            {
                throw new NotFoundException($"Pizza with ID {pizzaId} not found.");
            }
            var toppings = pizza.PizzaToppings
                            .Select(pt => pt.Topping);

            return toppings;
        }
        public async Task<Pizza> UpdateAsync(Guid id, Pizza pizzaToUpdate)
        {
            Pizza pizza = await _uow.PizzaRepository.GetByIdAsync(id);
            if (pizza != null)
            {
                pizza.Name = pizzaToUpdate.Name;
                pizza.PizzaToppings = pizzaToUpdate.PizzaToppings;
                pizza.PizzaPrices = pizzaToUpdate.PizzaPrices;
            }
            return await _uow.PizzaRepository.UpdateAsync(pizzaToUpdate);
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            Pizza pizzaFound = await _uow.PizzaRepository.GetByIdAsync(id);
            await _uow.PizzaRepository.DeleteAsync(pizzaFound);
            return await _uow.PizzaRepository.GetByIdAsync(id) == null;
        }
    }
}
