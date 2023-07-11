using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Truextend.PizzaTest.Data.Models;
using Truextend.PizzaTest.Data.Repository.Base;

namespace Truextend.PizzaTest.Data.Repository.Interfaces
{
    public interface IToppingRepository : IRepository<Topping>
    {
        Task<Topping> GetPizzasWithToppingByIdAsync(Guid id);
    }
}
