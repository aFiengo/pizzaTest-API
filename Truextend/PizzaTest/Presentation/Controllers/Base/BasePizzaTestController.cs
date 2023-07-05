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
        [Route("{Id}")]
        public async Task<IActionResult> GetItemByID([FromRoute] Guid Id)
        {
            T itemDTO = await _classManager.GetByIdAsync(Id);
            return Ok(new MiddlewareResponse<T>(itemDTO));
        }

        [HttpPost]
        [Route("")]
        public virtual async Task<IActionResult> AddItem([FromBody] T item)
        {
            T status = await _classManager.CreateAsync(item);
            return Ok(new MiddlewareResponse<T>(status));
        }

        [HttpPut("{Id}")]
        public virtual async Task<IActionResult> UpdateById([FromRoute] Guid Id, [FromBody] T item)
        {
            T status = await _classManager.UpdateAsync(Id, item);
            return Ok(new MiddlewareResponse<T>(status));
        }

    }
}
