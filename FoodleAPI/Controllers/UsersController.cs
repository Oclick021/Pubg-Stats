using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using PubgAPI.Services;

namespace PubgAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }




        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]User userParam)
        {
            var user = _userService.Authenticate(userParam.Username, userParam.Password);
            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            //using (var context = new FoodleContext())
            //{
            //    user.Password = UserService.Encrypt(user.Password);
            //    if (context.Users.Where(x => x.Username == user.Username).FirstOrDefault() == null)
            //    {
            //        context.Users.Add(user);
            //        context.SaveChanges();
            //        _userService.SignToken(user);
            //        user.Password = null;
            //        return Ok(user);

            //    }
            //}

            return BadRequest(new { message = "Username Already exists" });

        }

        [HttpGet("user")]
        public async Task<ActionResult<User>> GetUser()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            if (int.TryParse(claimsIdentity.FindFirst(ClaimTypes.Name)?.Value, out int userID))
            {
                User currentUser = await UserService.GetByID(userID);
                if (currentUser != null)
                {
                    currentUser.Password = null;
                    return currentUser;
                }
            }
            return NotFound();
        }
    }
}
