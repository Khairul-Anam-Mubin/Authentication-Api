using Authentication.Business.Interfaces;
using Authentication.Shared.Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        public IUserService UserService { get; set; }
        public UsersController(IUserService userService)
        {
            UserService = userService;
        }
        [HttpGet]
        public IActionResult Users()
        {
            return Ok(UserService.GetAllUsers());
        }
    }
}