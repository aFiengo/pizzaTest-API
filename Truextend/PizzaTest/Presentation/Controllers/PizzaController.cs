using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection;
using Truextend.PizzaTest.Logic.Exceptions;
using Truextend.PizzaTest.Logic.Managers.Interface;
using Truextend.PizzaTest.Logic.Models;
using Truextend.PizzaTest.Presentation.Controllers.Base;
using Truextend.PizzaTest.Presentation.Middleware;
using static ServiceStack.LicenseUtils;
using Microsoft.OpenApi;

namespace Truextend.PizzaTest.Presentation.Controllers
{
    [Produces("application/json")]
    [Route("api/pizzas")]
    public class PizzaController : BasePizzaTestController<PizzaDTO>
    {
        private readonly IPizzaManager _pizzaManager;
        public PizzaController(IPizzaManager pizzaManager) :base(pizzaManager) 
        {
            _pizzaManager = pizzaManager;
        }

        /// <summary>
        /// Retrieves all toppings for a specific pizza.
        /// </summary>
        /// <param name="id">The ID of the pizza.</param>
        /// <returns>A list of toppings for the pizza with the given ID.</returns>
        /// <response code="200">Returns the list of toppings for the given pizza.</response>
        [HttpGet]
        [Route("{id}/toppings")]
        public async Task<IActionResult> GetToppingsForPizzaAsync([FromRoute] Guid id)
        {
            var toppings = await _pizzaManager.GetToppingsForPizzaAsync(id);
            return Ok(new MiddlewareResponse<IEnumerable<ToppingDTO>>(toppings));
        }

        /// <summary>
        /// Adds a topping to a pizza.
        /// </summary>
        /// <param name="pizzaId">The ID of the pizza.</param>
        /// <param name="toppingId">The ID of the topping.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        /// <response code="200">The topping was successfully added to the pizza.</response>

        [HttpPut]
        [Route("{pizzaId}/toppings/{toppingId}")]
        public async Task<IActionResult> AddToppingToPizza([FromRoute] Guid pizzaId, [FromRoute] Guid toppingId)
        {
            Dictionary<string, object> result = await _pizzaManager.AddToppingToPizzaAsync(pizzaId, toppingId);
            return Ok(new MiddlewareResponse<Dictionary<string, object>>(result));
        }

        /// <summary>
        /// Removes a topping from a pizza.
        /// </summary>
        /// <param name="pizzaId">The ID of the pizza.</param>
        /// <param name="toppingId">The ID of the topping.</param>
        /// <returns>A response indicating whether the operation was successful or not.</returns>
        /// <response code="200">The topping was successfully removed from the pizza.</response>
        /// <response code="400">The operation failed.</response>
        [HttpDelete]
        [Route("{pizzaId}/toppings/{toppingId}")]
        public async Task<IActionResult> DeleteToppingFromPizza([FromRoute] Guid pizzaId, [FromRoute] Guid toppingId)
        {
            var isToppingDeleted = await _pizzaManager.DeleteToppingFromPizzaAsync(pizzaId, toppingId);
            string successMessage = "Successfully deleted";
            string errorMessage = "Failed to delete";
            if (isToppingDeleted)
            {
                return Ok(new MiddlewareResponse<bool>(isToppingDeleted, successMessage));
            }
            else if (!isToppingDeleted)
            {
                return NotFound(new MiddlewareResponse<bool>(false, "Pizza or topping not found"));
            }
            else
            {
                return BadRequest(new MiddlewareResponse<bool>(isToppingDeleted, errorMessage));
            }

        }

    }
}
