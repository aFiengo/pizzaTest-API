using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Truextend.PizzaTest.Data.Models;

namespace Truextend.PizzaTest.Logic.Managers.Interface
{
    public interface IToppingManager
    {
        Task<IEnumerable<Topping>> GetAllAsync();
        Task<Topping> GetByIdAsync(Guid id);
        Task<Topping> CreateAsync(Topping topping);
        Task<Topping> UpdateAsync(Guid id, Topping toppingToUpdate);
        Task<bool> DeleteAsync(Guid id);
    }
}
