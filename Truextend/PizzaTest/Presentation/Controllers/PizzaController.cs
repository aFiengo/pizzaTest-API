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
    public class PizzaController : ControllerBase
    {
        private readonly IPizzaManager _pizzaManager;
        public PizzaController(IPizzaManager pizzaManager)
        {
            _pizzaManager = pizzaManager;
        }
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllPizzasAsync()
        {
            var result = await _pizzaManager.GetAllPizzaAsync();
            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetPizzaByIdAsync(Guid id)
        {
            var result = await _pizzaManager.GetPizzaByIdAsync(id);
            return Ok(result);
        }

        [HttpGet]
        [Route("{id}/toppings")]
        public async Task<IActionResult> GetToppingsForPizzaAsync([FromRoute] Guid id)
        {
            var toppings = await _pizzaManager.GetToppingsForPizzaAsync(id);
            return Ok(new MiddlewareResponse<IEnumerable<ToppingDTO>>(toppings));
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreatePizzaAsync(PizzaCreateDTO pizzaToCreate)
        {
            var result = await _pizzaManager.CreatePizzaAsync(pizzaToCreate);
            return Ok(result);
        }

        [HttpPut]
        [Route("{pizzaId}/toppings/{toppingId}")]
        public async Task<IActionResult> AddToppingToPizzaAsync([FromRoute] Guid pizzaId, [FromRoute] Guid toppingId)
        {
            PizzaDTO updatedPizza = await _pizzaManager.AddToppingToPizzaAsync(pizzaId, toppingId);
            return Ok(new MiddlewareResponse<PizzaDTO>(updatedPizza));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeletePizza([FromRoute] Guid id)
        {
            var deletedPizza = await _pizzaManager.DeletePizzaAsync(id);
            if (deletedPizza == null)
            {
                return NotFound();
            }
            return Ok(deletedPizza);
        }

        [HttpDelete]
        [Route("{id}/toppings/{toppingId}")]
        public async Task<IActionResult> DeleteToppingFromPizza([FromRoute] Guid pizzaId, [FromRoute] Guid toppingId)
        {
            var deletedPizza = await _pizzaManager.DeleteToppingFromPizzaAsync(pizzaId, toppingId);
            if (deletedPizza == null)
            {
                return NotFound();
            }
            return Ok(deletedPizza);
        }
    }
}
