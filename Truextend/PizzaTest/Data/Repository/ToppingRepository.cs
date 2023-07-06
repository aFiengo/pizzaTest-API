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
    public class ToppingRepository : Repository<Topping>, IToppingRepository
    {
        public ToppingRepository(PizzaDbContext pizzaDbContext) : base(pizzaDbContext) { }
        public async Task<Topping> Delete(Guid id)
        {
            var topping = await dbContext.Topping.FindAsync(id);
            if (topping != null)
            {
                var pizzas = dbContext.Pizza
                    .Include(p => p.Toppings)
                    .Where(p => p.Toppings.Any(t => t.Id == id));

                foreach (var pizza in pizzas)
                {
                    pizza.Toppings.Remove(topping);
                }

                dbContext.Topping.Remove(topping);

                await dbContext.SaveChangesAsync();
            }
            return topping;
        }

    }
}
