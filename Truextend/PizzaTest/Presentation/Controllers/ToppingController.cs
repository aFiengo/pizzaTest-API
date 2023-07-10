using Microsoft.AspNetCore.Mvc;
using Truextend.PizzaTest.Logic.Managers;
using Truextend.PizzaTest.Logic.Managers.Interface;
using Truextend.PizzaTest.Logic.Models;
using Truextend.PizzaTest.Presentation.Controllers.Base;

namespace Truextend.PizzaTest.Presentation.Controllers
{
    [Produces("application/json")]
    [Route("api/toppings")]
    public class ToppingController : BasePizzaTestController<ToppingDTO>
    {
        private readonly IToppingManager _toppingManager;
        public ToppingController(IToppingManager toppingManager) : base(toppingManager) 
        {
            _toppingManager = toppingManager;
        }

       
    }
}
