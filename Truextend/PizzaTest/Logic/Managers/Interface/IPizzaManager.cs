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
    public interface IPizzaManager
    {
        Task<IEnumerable<PizzaNameDTO>> GetAllPizzaAsync();
        Task<PizzaNameDTO> GetPizzaByIdAsync(Guid id);
        Task<PizzaDTO> CreatePizzaAsync(PizzaCreateDTO pizzaToCreate);
        Task<PizzaDTO> AddToppingToPizzaAsync(Guid pizzaId, Guid toppingId);
        Task<IEnumerable<ToppingDTO>> GetToppingsForPizzaAsync(Guid pizzaId);
        Task<PizzaDTO> UpdatePizzaAsync(Guid id, PizzaCreateDTO pizzaToUpdate);
        Task<PizzaDTO> DeletePizzaAsync(Guid id);
    }
}
