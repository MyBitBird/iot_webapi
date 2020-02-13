using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IOT.DTO;
using Microsoft.AspNetCore.Authorization;
using IOT.Models;
using IOT.Services;
using IOT.Helper;
using Microsoft.Extensions.Configuration;
using AutoMapper;

namespace IOT.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly ServiceService _serviceService;

        public UserController(IOTContext context, IMapper mapper, IConfiguration config)
        {
            _service = new UserService(context);
            _config = config;
            _mapper = mapper;
            _serviceService = new ServiceService(context);
        }

        [HttpPost("Authenticate")]
        [AllowAnonymous]
        public IActionResult Authenticate([FromBody] AuthorizeDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var loginUser = _service.Authenticate(dto.Username, Utility.HashPassword(dto.Password, _config));
            if (loginUser == null)
                return Unauthorized();

            return Ok(new
            {
                loginUser.Id,
                token = Utility.BuildToken(loginUser, _config, loginUser.Type == MyEnums.UserTypes.ADMIN ? "ADMIN" : "DEVICE")
            });
        }

        [HttpGet("Refresh")]
        [Authorize]
        public async Task<IActionResult> RefreshToken()
        {
            var logedInUser = await _service.GetById(Utility.GetCurrentUserId(User));
            return Ok(new { token = Utility.BuildToken(logedInUser, _config, Utility.GetCurrentUserRole(User)) });
        }

        [HttpPost("SignUp"), AllowAnonymous]
        public async Task<IActionResult> SignUp([FromBody] UserDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = _mapper.Map<Users>(dto);
            user = await _service.SignUp(user);
            return Ok(new { user.Id, token = Utility.BuildToken(user, _config, "ADMIN") });
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> NewUser([FromBody] UserDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var userId = Utility.GetCurrentUserId(User);
            var user = _mapper.Map<Users>(dto);

            user = await _service.AddUser(user, userId, await _serviceService.GetByUserId(userId));

            if (user == null)
                return Forbid();
            return Ok(new { user.Id, token = Utility.BuildToken(user, _config, "DEVICE") });
        }

        [HttpPut("Profile")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> EditProfile([FromBody] UserDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var userId = Utility.GetCurrentUserId(User);
            var user = _mapper.Map<Users>(dto);

            var result = await _service.EditProfile(userId, user, Utility.HashPassword(dto.OldPassword, _config));
            if (result) return Ok();
            return StatusCode(403, new { result = "Wrong Password!" });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdateSubUsers(Guid id, [FromBody] UserDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var parentId = Utility.GetCurrentUserId(User);
            var user = _mapper.Map<Users>(dto);

            var result = await _service.UpdateSubUsers(id, parentId, user, await _serviceService.GetByUserId(parentId));
            if (result) return Ok();
            return StatusCode(403, new { result = "Invalid Access" });
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public async Task<IActionResult> GetSubUsers()
        {
            var parentId = Utility.GetCurrentUserId(User);
            return Ok(_mapper.Map<UserDTO[]>(await _service.GetSubUsers(parentId)));
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubUser(Guid id)
        {
            var parentId = Utility.GetCurrentUserId(User);
            return Ok(await _service.DeleteSubUser(id, parentId));
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var parentId = Utility.GetCurrentUserId(User);
            return Ok(_mapper.Map<UserDTO>(await _service.GetByIdAndParentId(id, parentId)));
        }
    }
}