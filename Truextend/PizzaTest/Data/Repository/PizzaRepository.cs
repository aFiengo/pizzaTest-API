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
        public async Task<IEnumerable<Pizza>> GetAllPizzaAsync()
        {
            return await dbContext.Pizza
                .Include(p => p.Toppings)
                .ToListAsync();
        }
        public async Task<Pizza> GetPizzaByIdAsync(Guid id)
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

            var topping = await dbContext.Topping.FindAsync(toppingId);
            if (topping == null)
            {
                return null;
            }

            pizza.Toppings.Add(topping);

            await dbContext.SaveChangesAsync();

            return pizza;
        }
        public void UpdatePizza(Pizza existingPizza, List<Topping> updatedToppings)
        {
            existingPizza.Toppings.Clear();

            foreach (var topping in updatedToppings)
            {
                if (!existingPizza.Toppings.Any(t => t.Id == topping.Id))
                {
                    existingPizza.Toppings.Add(topping);
                }
            }
        }
        public async Task<Pizza> GetPizzaWithToppingsAsync(Guid id)
        {
            var pizza = await dbContext.Pizza
                .Include(p => p.Toppings)
                .FirstOrDefaultAsync(p => p.Id == id);
            return pizza;
        }
        public void Delete(Pizza pizza)
        {
            dbContext.Pizza.Remove(pizza);
        }
        
    }
}
