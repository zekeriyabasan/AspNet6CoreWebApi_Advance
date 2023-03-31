using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Template_Api.Models;

namespace Template_Api.Controllers
{
    [Route("home")]
    [ApiController]
    public class homeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new ResponseModel { StatusCode = 200, Message = "Hello!" });
        }
    }
}
