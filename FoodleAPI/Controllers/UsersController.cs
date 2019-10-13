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
using PubgAPI.Models;
using AutoMapper;
using PubgAPI.Dtos;
using PubgAPI.Helpers;

namespace PubgAPI.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private IUserService _userService;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }




        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Authenticate([FromBody]User userParam)
        {
            var user = _userService.Authenticate(userParam.Username, userParam.Password);
            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            return Ok(_mapper.Map<UserDto>(user));
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem();
            }


            using (var context = new ApiDbContext())
            {
                user.Password = UserService.Encrypt(user.Password);
                if (context.Users.Where(x => x.Username == user.Username).FirstOrDefault() == null)
                {
                    user.Role = Role.GeneralUser;
                    context.Users.Add(user);
                    context.SaveChanges();
                    _userService.SignToken(user);
                    return Ok(_mapper.Map<UserDto>(user));
                }
                return BadRequest(new { message = "Username Already exists" });
            }


        }

        [HttpGet("UserProfile")]
        public async Task<ActionResult<UserDto>> GetUserProfile()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            if (int.TryParse(claimsIdentity.FindFirst(ClaimTypes.Name)?.Value, out int userID))
            {
                User currentUser = await UserService.GetByID(userID);
                if (currentUser != null)
                {
                    var user = _mapper.Map<UserDto>(currentUser);
                    currentUser.Password = null;
                    return user;
                }
            }
            return NotFound();
        }

        [Authorize(Roles =Role.Admin+","+Role.Moderator)]
        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            using (var con = new ApiDbContext())
            {
                var users =await con.Users.ToListAsync();

                var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
                return Ok(userDtos);
            }
        }
    }
}
