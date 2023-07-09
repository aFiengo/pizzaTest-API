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
    public interface IPizzaManager 
    {
        Task<IEnumerable<PizzaNameDTO>> GetAllAsync();
        Task<PizzaNameDTO> GetByIdAsync(Guid id);
        Task<PizzaDTO> AddToppingToPizzaAsync(Guid pizzaId, Guid toppingId);
        Task<IEnumerable<ToppingDTO>> GetToppingsForPizzaAsync(Guid pizzaId);
        Task<PizzaDTO> UpdatePizzaAsync(Guid id, PizzaCreateDTO pizzaToUpdate);
        Task<PizzaDTO> DeletePizzaAsync(Guid id);
        Task<PizzaDTO> DeleteToppingFromPizzaAsync(Guid pizzaId, Guid toppingId);
    }
}
