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
        Task<PizzaDTO> AddToppingToPizzaAsync(Guid pizzaId, Guid toppingId);
        Task<IEnumerable<ToppingDTO>> GetToppingsForPizzaAsync(Guid pizzaId);
        Task<bool> DeleteToppingFromPizzaAsync(Guid pizzaId, Guid toppingId);
    }
}
