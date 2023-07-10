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
        /// <summary>
        /// Retrieves all items from the database.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/items
        ///
        /// </remarks>
        /// <returns>A list of all items in the database.</returns>
        /// <response code="200">Returns the list of items.</response>
        [HttpGet]
        public virtual async Task<IActionResult> GetAllItems()
        {
            return Ok(new MiddlewareResponse<IEnumerable<T>>(await _classManager.GetAllAsync()));
        }

        /// <summary>
        /// Retrieves a specific item by its ID.
        /// </summary>
        /// <param name="id">The ID of the item.</param>
        /// <returns>The item with the given ID.</returns>
        /// <response code="200">Returns the item with the given ID.</response>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetItemByID([FromRoute] Guid id)
        {
            T itemDTO = await _classManager.GetByIdAsync(id);
            return Ok(new MiddlewareResponse<T>(itemDTO));
        }

        /// <summary>
        /// Adds a new item.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <returns>The added item.</returns>
        /// <response code="200">Returns the added item.</response>
        [HttpPost]
        [Route("")]
        public virtual async Task<IActionResult> AddItem([FromBody] T item)
        {
            T status = await _classManager.CreateAsync(item);
            return Ok(new MiddlewareResponse<T>(status));
        }

        /// <summary>
        /// Updates an existing item.
        /// </summary>
        /// <param name="id">The ID of the item to update.</param>
        /// <param name="item">The updated information of the item.</param>
        /// <returns>The updated item.</returns>
        /// <response code="200">Returns the updated item.</response>
        [HttpPut]
        [Route("{id}")]
        public virtual async Task<IActionResult> UpdateById([FromRoute] Guid id, [FromBody] T item)
        {
            T status = await _classManager.UpdateAsync(id, item);
            return Ok(new MiddlewareResponse<T>(status));
        }

        /// <summary>
        /// Deletes an item by its ID.
        /// </summary>
        /// <param name="id">The ID of the item to delete.</param>
        /// <returns>A response indicating whether the operation was successful or not.</returns>
        /// <response code="200">The item was successfully deleted.</response>
        /// <response code="400">The deletion failed.</response>
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
