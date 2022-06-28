using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Authentication.Api.Interfaces;
using Authentication.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
            if (!UserService.IsUserEmailAndPasswordExist(loginModel)) return Forbid("Wrong Credentials");
            return Ok(AuthService.CreateTokenByLoginModel(loginModel));
        }
        [HttpPost]
        [Route("tokenRefresh")]
        public IActionResult GetTokenModel([FromBody] TokenModel tokenModel)
        {
            return Ok(AuthService.GetTokenModelByTokenModel(tokenModel));
        }
    }
}