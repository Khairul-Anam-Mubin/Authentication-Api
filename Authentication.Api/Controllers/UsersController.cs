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
        [HttpPost]
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
        [HttpPut]
        [Authorize]
        public IActionResult UpdateUser([FromBody] UserModel userModel)
        {
            try
            {
                var updatedUserModel = UserService.UpdateUser(userModel);
                return Ok(updatedUserModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteUser(string id)
        {
            try
            {
                var deletedUserModel = UserService.DeleteUser(id);
                return Ok(deletedUserModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetUser(string id)
        {
            try
            {
                var userModel = UserService.GetUser(id);
                return Ok(userModel);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet]
        [Authorize]
        public IActionResult Users()
        {
            return Ok(UserService.GetAllUsers());
        }
        
    }
}