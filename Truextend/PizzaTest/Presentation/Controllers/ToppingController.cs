using Truextend.PizzaTest.Logic.Managers.Base;
using Truextend.PizzaTest.Logic.Models;
using Truextend.PizzaTest.Presentation.Controllers.Base;

namespace Truextend.PizzaTest.Presentation.Controllers
{
    public class ToppingController : BasePizzaTestController<ToppingDTO>
    {
        public ToppingController(IGenericManager<ToppingDTO> toppingManager) : base(toppingManager)
        {
        }
    }
}
