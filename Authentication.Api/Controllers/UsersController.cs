using Authentication.Api.Interfaces;
using Authentication.Api.Models;
using Microsoft.AspNetCore.Authorization;
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
            try
            {
                var createdUserModel = UserService.CreateUser(userModel);
                return Ok(createdUserModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}