using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection;
using Truextend.PizzaTest.Logic.Exceptions;
using Truextend.PizzaTest.Logic.Managers.Interface;
using Truextend.PizzaTest.Logic.Models;
using Truextend.PizzaTest.Presentation.Controllers.Base;
using Truextend.PizzaTest.Presentation.Middleware;
using static ServiceStack.LicenseUtils;

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

        
        [HttpGet]
        [Route("{id}/toppings")]
        public async Task<IActionResult> GetToppingsForPizzaAsync([FromRoute] Guid id)
        {
            var toppings = await _pizzaManager.GetToppingsForPizzaAsync(id);
            return Ok(new MiddlewareResponse<IEnumerable<ToppingDTO>>(toppings));
        }


        [HttpPut]
        [Route("{pizzaId}/toppings/{toppingId}")]
        public async Task<IActionResult> AddToppingToPizza([FromRoute] Guid pizzaId, [FromRoute] Guid toppingId)
        {
            Dictionary<string, object> result = await _pizzaManager.AddToppingToPizzaAsync(pizzaId, toppingId);
            return Ok(new MiddlewareResponse<Dictionary<string, object>>(result));
        }


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
                else
                {
                    return BadRequest(new MiddlewareResponse<bool>(isToppingDeleted, errorMessage));
                }
           
        }

    }
}
