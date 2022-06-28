using Authentication.Api.Interfaces;
using Authentication.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public IUserService UserService { get; set; }
        public UsersController(IUserService userService)
        {
            UserService = userService;
        }
        [HttpGet]
        [Authorize]
        public IActionResult Users()
        {
            return Ok(UserService.GetAllUsers());
        }
        [HttpPost]
        [Route("createUser")]
        public IActionResult CreateUser([FromBody] UserModel userModel)
        {
            userModel = UserService.CreateUser(userModel);
            return Ok(userModel);
        }
    }
}