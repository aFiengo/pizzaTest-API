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
        public async Task<IEnumerable<Pizza>> GetAllAsync()
        {
            return await dbContext.Pizza
                .Include(p => p.Toppings)
                .ToListAsync();
        }
        public async Task<Pizza> GetByIdAsync(Guid id)
        {
            return await dbContext.Pizza
                .Include(p => p.Toppings)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<IEnumerable<Topping>> GetToppingsForPizzaAsync(Guid pizzaId)
        {
            var pizza = await dbContext.Pizza
                .Include(p => p.Toppings)
                .FirstOrDefaultAsync(p => p.Id == pizzaId);

            if (pizza == null)
                return null;

            return pizza.Toppings;
        }
        public async Task<Pizza> AddToppingToPizzaAsync(Guid pizzaId, Guid toppingId)
        {
            var pizza = await dbContext.Pizza
            .Include(p => p.Toppings)
            .FirstOrDefaultAsync(p => p.Id == pizzaId);

            if (pizza == null)
            {
                return null;
            }

            var topping = await dbContext.Topping.FindAsync(toppingId);
            if (topping == null)
            {
                return null;
            }

            if (!pizza.Toppings.Any(t => t.Id == toppingId))
            {
                pizza.Toppings.Add(topping);
            }

            await dbContext.SaveChangesAsync();

            return pizza;
        }
        public async Task<Pizza> DeleteToppingFromPizzaAsync(Guid pizzaId, Guid toppingId)
        {
            var pizza = await dbContext.Pizza.FindAsync(pizzaId);
            if (pizza == null)
            {
                return null;
            }

            var toppingToRemove = pizza.Toppings.FirstOrDefault(t => t.Id == toppingId);
            if (toppingToRemove == null)
            {
                return null;
            }

            pizza.Toppings.Remove(toppingToRemove);
            await dbContext.SaveChangesAsync();

            return pizza;
        }



    }
}
