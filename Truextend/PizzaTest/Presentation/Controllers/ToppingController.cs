﻿using Microsoft.AspNetCore.Mvc;
using Truextend.PizzaTest.Logic.Managers;
using Truextend.PizzaTest.Logic.Managers.Interface;
using Truextend.PizzaTest.Logic.Models;
using Truextend.PizzaTest.Presentation.Controllers.Base;

namespace Truextend.PizzaTest.Presentation.Controllers
{
    [Produces("application/json")]
    [Route("api/toppings")]
    public class ToppingController : ControllerBase
    {
        private readonly IToppingManager _toppingManager;
        public ToppingController(IToppingManager toppingManager)
        {
            _toppingManager = toppingManager;
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteTopping([FromRoute] Guid id)
        {
            var deletedTopping = await _toppingManager.DeleteAsync(id);
            if (deletedTopping == null)
            {
                return NotFound();
            }
            return Ok(deletedTopping);
        }
    }
}
