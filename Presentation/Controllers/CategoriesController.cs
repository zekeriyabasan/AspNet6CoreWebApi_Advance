using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController:ControllerBase
    {
        IServiceManager _manager;

        public CategoriesController(IServiceManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            return Ok(await _manager.CategoryService.GetAllCategoriesAsync(false));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetACategoryById([FromRoute] int id)
        {
            return Ok(await _manager.CategoryService.GetACategoryById(id,false));
        }
    }
}
