using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Truextend.PizzaTest.Data.Models;
using Truextend.PizzaTest.Logic.Managers;
using Truextend.PizzaTest.Logic.Models;

namespace Truextend.PizzaTest.Logic.Managers.Interface
{
    public interface IToppingManager 
    {
        Task<IEnumerable<ToppingDTO>> GetAllAsync();
        Task<ToppingDTO> GetByIdAsync(Guid id);
        Task<ToppingDTO> CreateAsync(ToppingDTO toppingToAdd);
        Task<ToppingDTO> UpdateAsync(Guid id, ToppingDTO toppingToUpdate);
        Task<ToppingDTO> DeleteTopping(Guid id);
    }
}
