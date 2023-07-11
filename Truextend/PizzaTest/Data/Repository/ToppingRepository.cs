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
        public async Task<Topping> GetPizzasWithToppingByIdAsync(Guid id)
        {
            return await dbContext.Topping
                .Include(t => t.Pizzas)
                .SingleOrDefaultAsync(t => t.Id == id);
        }

    }
}
