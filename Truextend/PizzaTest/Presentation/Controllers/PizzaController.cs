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
            try
            {
                var toppings = await _pizzaManager.GetToppingsForPizzaAsync(id);
                return Ok(new MiddlewareResponse<IEnumerable<ToppingDTO>>(toppings));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new MiddlewareResponse<string>(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new MiddlewareResponse<string>(ex.Message));
            }
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
            try
            {
                Dictionary<string, object> result = await _pizzaManager.AddToppingToPizzaAsync(pizzaId, toppingId);
                return Ok(new MiddlewareResponse<Dictionary<string, object>>(result));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new MiddlewareResponse<string>(ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new MiddlewareResponse<string>(ex.Message));
            }
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
            try
            {
                var isToppingDeleted = await _pizzaManager.DeleteToppingFromPizzaAsync(pizzaId, toppingId);
                string successMessage = "Successfully deleted";
                if (isToppingDeleted)
                {
                    return Ok(new MiddlewareResponse<bool>(isToppingDeleted, successMessage));
                }
                else
                {
                    return BadRequest(new MiddlewareResponse<bool>(isToppingDeleted, "Failed to delete"));
                }
            }
            catch (NotFoundException ex)
            {
                return NotFound(new MiddlewareResponse<bool>(false, "Error", ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new MiddlewareResponse<bool>(false, "Error", ex.Message));
            }
        }
    }
}
