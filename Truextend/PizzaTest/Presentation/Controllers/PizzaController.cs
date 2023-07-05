using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using Truextend.PizzaTest.Logic.Managers.Interface;
using Truextend.PizzaTest.Logic.Models;
using Truextend.PizzaTest.Presentation.Controllers.Base;
using Truextend.PizzaTest.Presentation.Middleware;

namespace Truextend.PizzaTest.Presentation.Controllers
{
    [Produces("application/json")]
    [Route("api/pizzas")]
    public class PizzaController : BasePizzaTestController<PizzaDTO>
    {
        private readonly IPizzaManager _pizzaManager;
        public PizzaController(IPizzaManager pizzaManager) : base(pizzaManager)
        {
            _pizzaManager = pizzaManager;
        }

        [HttpPost("{Id}/toppings/{toppingId}")]
        public async Task<IActionResult> AddToppingToPizzaAsync([FromRoute] Guid pizzaId, [FromRoute] Guid toppingId)
        {
            PizzaDTO updatedPizza = await _pizzaManager.AddToppingToPizzaAsync(pizzaId, toppingId);
            return Ok(new MiddlewareResponse<PizzaDTO>(updatedPizza));
        }

        [HttpGet("{Id}/toppings")]
        public async Task<IActionResult> GetToppingsForPizzaAsync([FromRoute] Guid id)
        {
            var toppings = await _pizzaManager.GetToppingsForPizzaAsync(id);
            return Ok(new MiddlewareResponse<IEnumerable<ToppingDTO>>(toppings));
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeletePizza([FromRoute] Guid id)
        {
            var deletedPizza = await _pizzaManager.DeletePizzaAsync(id);
            if (deletedPizza == null)
            {
                return NotFound();
            }
            return Ok(deletedPizza);
        }
    }
}
