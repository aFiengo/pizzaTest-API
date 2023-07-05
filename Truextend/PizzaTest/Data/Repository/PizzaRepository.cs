using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Truextend.PizzaTest.Data.Exceptions;
using Truextend.PizzaTest.Data.Models;
using Truextend.PizzaTest.Data.Repository.Base;
using Truextend.PizzaTest.Data.Repository.Interfaces;

namespace Truextend.PizzaTest.Data.Repository
{
    public class PizzaRepository : Repository<Pizza>, IPizzaRepository
    {
        public PizzaRepository(PizzaDbContext pizzaDbContext) : base(pizzaDbContext) { }
        public async Task<IEnumerable<Topping>> GetToppingsForPizzaAsync(Guid pizzaId)
        {
            var pizza = await dbContext.Pizza
                .Include(p => p.PizzaToppings)
                    .ThenInclude(pt => pt.Topping)
                .FirstOrDefaultAsync(p => p.Id == pizzaId);

            if (pizza == null)
                return null;

            var toppings = pizza.PizzaToppings
                .Select(pt => pt.Topping);

            return toppings;
        }

        public async Task<Pizza> AddToppingToPizzaAsync(Guid pizzaId, Guid toppingId)
        {
            try
            {
                var pizza = await dbContext.Pizza
                    .Include(p => p.PizzaToppings)
                    .FirstOrDefaultAsync(p => p.Id == pizzaId);

                if (pizza == null)
                    throw new DatabaseException($"Pizza with ID {pizzaId} not found.");

                var topping = await dbContext.Topping.FindAsync(toppingId);
                if (topping == null)
                    throw new DatabaseException($"Topping with ID {toppingId} not found.");

                var pizzaTopping = new PizzaTopping { PizzaId = pizzaId, ToppingId = toppingId };
                await dbContext.PizzaTopping.AddAsync(pizzaTopping);

                await dbContext.SaveChangesAsync();

                return pizza;
            }
            catch (Exception e)
            {
                throw new DatabaseException("Error adding topping to pizza: " + e.Message);
            }
        }
    }
}
