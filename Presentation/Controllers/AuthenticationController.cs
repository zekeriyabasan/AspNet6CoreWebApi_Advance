using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Contracts;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public AuthenticationController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> RegiterUser([FromBody] UserForRegistrationDto userForRegistrationDto)
        {
            var result = await _serviceManager.AuthenticationService.RegisterUser(userForRegistrationDto);
            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    ModelState.TryAddModelError(err.Code, err.Description);

                }
                return BadRequest(ModelState);

            }
            return StatusCode(201);
        }

        [HttpPost("Login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto userForAuthDto)
        {
            if(!await _serviceManager.AuthenticationService.ValidateUser(userForAuthDto))
                return Unauthorized();

            var tokenDto = await _serviceManager.AuthenticationService.CreateToken(populateExp: true);

            return Ok(tokenDto);
   
        }

        [HttpPost("Refresh")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Refresh([FromBody] TokenDto tokenDto)
        {
            var tokenDtoReturn = await _serviceManager.AuthenticationService.RefreshToken(tokenDto);

            return Ok(tokenDtoReturn);
        }

    }
}
