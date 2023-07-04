using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using Truextend.PizzaTest.Logic.Managers.Interface;
using Truextend.PizzaTest.Logic.Models;

namespace Truextend.PizzaTest.Presentation.Controllers
{
    [Produces("application/json")]
    [Route("api/pizzas")]
    public class PizzaController : Controller
    {
        private readonly IPizzaManager _pizzaManager;
        public PizzaController(IPizzaManager pizzaManager)
        {
            _pizzaManager= pizzaManager;
        }


        [HttpGet]
        [Route("")]
        public async Task<ActionResult> GetAllPizzasAsync()
        {
            IEnumerable<PizzaDTO> pizzaDto = await _pizzaManager.GetAllAsync();
        }
    }
}
