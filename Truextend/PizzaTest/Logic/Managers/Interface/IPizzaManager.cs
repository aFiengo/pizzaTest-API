using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Truextend.PizzaTest.Data.Models;
using Truextend.PizzaTest.Logic.Managers.Base;
using Truextend.PizzaTest.Logic.Models;

namespace Truextend.PizzaTest.Logic.Managers.Interface
{
    public interface IPizzaManager : IGenericManager<PizzaDTO>
    {
        Task<IEnumerable<PizzaDTO>> GetAllAsync();
        Task<PizzaDTO> GetByIdAsync(Guid id);
        Task<PizzaDTO> CreateAsync(PizzaDTO pizzaToAdd);
        Task<PizzaDTO> AddToppingToPizzaAsync(Guid pizzaId, Guid toppingId);
        Task<IEnumerable<ToppingDTO>> GetToppingsForPizzaAsync(Guid pizzaId);
        Task<PizzaDTO> UpdateAsync(Guid id, PizzaDTO pizzaToUpdate);
        Task<bool> DeleteAsync(Guid id);
    }
}
