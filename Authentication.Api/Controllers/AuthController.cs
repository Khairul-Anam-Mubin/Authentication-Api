using Authentication.Api.Interfaces;
using Authentication.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public IUserService UserService { get; set; }
        public IAuthService AuthService { get; set; }
        public IConfiguration Configuration { get; set; }

        public AuthController(IUserService userService, IConfiguration configuration, IAuthService authService)
        {
            UserService = userService;
            Configuration = configuration;
            AuthService = authService;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult UserLogin([FromBody] LoginModel loginModel)
        {
            try
            {
                var token = AuthService.GetTokenModel(loginModel);
                return Ok(token);
            }
            catch (Exception e)
            {
               return BadRequest(e.Message);
            }
        }
        [HttpPost]
        [Route("token")]
        public IActionResult GetTokenModel([FromBody] TokenModel tokenModel)
        {
            try
            {
                var token = AuthService.GetTokenModel(tokenModel);
                return Ok(token);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}