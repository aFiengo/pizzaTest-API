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
            var pizza = await dbContext.Pizza
            .Include(p => p.PizzaToppings)
            .FirstOrDefaultAsync(p => p.Id == pizzaId);

            var topping = await dbContext.Topping.FindAsync(toppingId);
            if (topping == null)
            {
                return null;
            }

            pizza.PizzaToppings.Add(new PizzaTopping { PizzaId = pizzaId, ToppingId = toppingId });

            await dbContext.SaveChangesAsync();

            return pizza;
        }
        public void UpdateToppings(Pizza existingPizza, List<Topping> updatedToppings)
        {
            existingPizza.PizzaToppings.RemoveAll(pt => !updatedToppings.Exists(t => t.Id == pt.ToppingId));

            foreach (var topping in updatedToppings)
            {
                if (!existingPizza.PizzaToppings.Exists(pt => pt.ToppingId == topping.Id))
                {
                    existingPizza.PizzaToppings.Add(new PizzaTopping { PizzaId = existingPizza.Id, ToppingId = topping.Id });
                }
            }
        }
    }
}
