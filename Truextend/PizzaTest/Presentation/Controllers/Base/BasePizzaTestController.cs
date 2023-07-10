using Microsoft.AspNetCore.Mvc;
using Truextend.PizzaTest.Logic.Managers.Base;
using Truextend.PizzaTest.Presentation.Middleware;

namespace Truextend.PizzaTest.Presentation.Controllers.Base
{
    [ApiController]
    public class BasePizzaTestController<T> : ControllerBase where T : class
    {
        private readonly IGenericManager<T> _classManager;
        public BasePizzaTestController(IGenericManager<T> ClassManager) 
        {
            _classManager = ClassManager;
        }

        [HttpGet]
        public virtual async Task<IActionResult> GetAllItems()
        {
            return Ok(new MiddlewareResponse<IEnumerable<T>>(await _classManager.GetAllAsync()));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetItemByID([FromRoute] Guid id)
        {
            T itemDTO = await _classManager.GetByIdAsync(id);
            return Ok(new MiddlewareResponse<T>(itemDTO));
        }

        [HttpPost]
        [Route("")]
        public virtual async Task<IActionResult> AddItem([FromBody] T item)
        {
            T status = await _classManager.CreateAsync(item);
            return Ok(new MiddlewareResponse<T>(status));
        }

        [HttpPut]
        [Route("{id}")]
        public virtual async Task<IActionResult> UpdateById([FromRoute] Guid id, [FromBody] T item)
        {
            T status = await _classManager.UpdateAsync(id, item);
            return Ok(new MiddlewareResponse<T>(status));
        }
        [HttpDelete]
        [Route("{id}")]
        public virtual async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            bool isDeleted = await _classManager.DeleteAsync(id);
            string successMessage = "Successfully deleted";
            string errorMessage = "Failed to delete";

            if (isDeleted)
            {
                return Ok(new MiddlewareResponse<bool>(isDeleted, successMessage));
            }
            else
            {
                return BadRequest(new MiddlewareResponse<bool>(isDeleted, errorMessage));
            }
        }

    }
}
