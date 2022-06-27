using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Authentication.Business.Interfaces;
using Authentication.Shared.Library.Models;

namespace Authentication.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public IUserService UserService { get; set; }
        public ITokenService TokenService { get; set; }
        public IConfiguration Configuration { get; set; }
        public AuthController(IUserService userService, IConfiguration configuration, ITokenService tokenService)
        {
            UserService = userService;
            Configuration = configuration;
            TokenService = tokenService;
        }
        [HttpPost]
        [Route("login")]
        public IActionResult UserLogin([FromBody] LoginModel loginModel)
        {
            var userModel = UserService.GetUserByEmail(loginModel.Email);
            if (userModel == null) return Forbid();
            if (userModel.Password != loginModel.Password) return Forbid();
            string signingKey = Configuration["JWT:SecretKey"];
            string issuer = Configuration["JWT:ValidIssuer"];
            string audience = Configuration["JWT:ValidAudience"];
            int accessTokenValidityInMinutes = Convert.ToInt32(Configuration["JWT:AccessTokenValidityInMinutes"]);
            int refreshTokenValidityInDays = Convert.ToInt32(Configuration["JWT:RefreshTokenValidityInDays"]);
            var tokenModel = TokenService.GenerateTokenModel(signingKey, issuer, audience, accessTokenValidityInMinutes,
                refreshTokenValidityInDays, new List<Claim>());
            TokenService.SaveTokenModel(tokenModel);
            return Ok(tokenModel);
        }

        [HttpPost]
        [Route("createUser")]
        public IActionResult CreateUser([FromBody] UserModel userModel)
        {
            userModel = UserService.CreateUser(userModel);
            return Ok(userModel);
        }

        [HttpPost]
        [Route("tokenRefresh")]
        public IActionResult CreateRefreshToken([FromBody] TokenModel tokenModel)
        {
            
            return Ok();
        }
    }
}