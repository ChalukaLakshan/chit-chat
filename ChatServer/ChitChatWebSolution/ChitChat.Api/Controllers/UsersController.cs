using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Chat.Data.EfCore;
using Chat.Domain.Interfaces;
using ChitChat.Api.Dto;
using Microsoft.AspNetCore.Authorization;

namespace ChitChat.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateUser([FromBody] UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            await _userService.CreateUserAsync(user);

            return Created(string.Empty, true);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);

            var token = await _userService.LoginAsync(user);

            return Ok(new AccessDto{Token = token, Username = userDto.Username});
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GelAllUsers()
        {
            var userList = (await _userService.GetAllUsersAsync()).ToList()
                .Select(u => new UserDto() {Username = u.Username});

            return Ok(userList);
        }

        [HttpGet("name/{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GelUsersByName([FromRoute] string username)
        {
            var user = await _userService.FindUserByUsernameAsync(username);
            return Ok(_mapper.Map<UserDto>(user));
        }
    }
}
