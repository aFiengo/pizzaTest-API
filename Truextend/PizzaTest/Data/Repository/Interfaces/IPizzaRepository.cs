using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Truextend.PizzaTest.Data.Models;
using Truextend.PizzaTest.Data.Repository.Base;

namespace Truextend.PizzaTest.Data.Repository.Interfaces
{
    public interface IPizzaRepository : IRepository<Pizza>
    {
        Task<IEnumerable<Pizza>> GetAllPizzaAsync();
        Task<Pizza> GetPizzaByIdAsync(Guid id);
        Task<Pizza>AddToppingToPizzaAsync(Guid pizzaId, Guid toppingId);
        Task<IEnumerable<Topping>> GetToppingsForPizzaAsync(Guid pizzaId);
        Task<Pizza> GetPizzaWithToppingsAsync(Guid id);
        void Delete(Pizza pizza);
    }
}
