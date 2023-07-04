using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Truextend.PizzaTest.Data.Models;

namespace Truextend.PizzaTest.Logic.Managers.Interface
{
    public interface IPizzaManager
    {
        Task<IEnumerable<Pizza>> GetAllAsync();
        Task<Pizza> GetByIdAsync(Guid id);
        Task<Pizza> CreateAsync(Pizza pizzaToAdd);
        Task<Pizza> AddToppingToPizzaAsync(Guid pizzaId, Guid toppingId);
        Task<IEnumerable<Topping>> GetToppingsForPizzaAsync(Guid pizzaId);
        Task<Pizza> UpdateAsync(Guid id, Pizza pizzaToUpdate);
        Task<bool> DeleteAsync(Guid id);
    }
}
