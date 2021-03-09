using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using BtlWebApi.Services;
using BtlWebApi.Models;

namespace BtlWebApi.Controllers
{
    // [Authorize]
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }
        
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public User Authenticate([FromBody] AuthModel model)
        {
            var user = _userService.Authenticate(model.Username, model.Password);
            return user;
        }

        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<User> GetAll()
        {
            var users = _userService.GetAll();
            return users;
        }

        [AllowAnonymous]
        [HttpGet("{id:length(24)}")]
        public User Get(string id)
        {
            var user = _userService.GetById(id);

            return user;
        }

    }
}
