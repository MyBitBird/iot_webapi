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
        UserService _service;
        IConfiguration _config;
        IMapper _mapper;

        ServiceService _serviceService;
        public UserController(IOTContext context,IMapper mapper,IConfiguration config)
        {
            _service = new UserService(context);
            _config=config;
            _mapper=mapper;
            _serviceService = new ServiceService(context);


        }

        [HttpPost("Authenticate")]
        [AllowAnonymous]
        public IActionResult Authenticate([FromBody] AuthorizeDTO dto )
        {
             if(!this.ModelState.IsValid)
                return BadRequest();

            Users login_user= _service.Authenticate(dto.Username,Utility.HashPassword(dto.Password, _config));
            if(login_user ==null) return Unauthorized();

            return Ok(new {login_user.Id,
                           token= Utility.BuildToken(login_user,_config,login_user.Type==(byte)MyEnums.UserTypes.ADMIN ? "ADMIN" : "DEVICE") });

        }

        [HttpGet("Refresh")]
        [Authorize]
        public async Task<IActionResult> RefreshToken()
        {
            Users logedInUser= await _service.GetById(Utility.GetCurrentUserID(User));
            return Ok(new {token = Utility.BuildToken(logedInUser, _config,Utility.GetCurrentUserRole(User))});
        }

        [HttpPost("SignUp"),AllowAnonymous]
        public async Task<IActionResult> SignUp([FromBody] UserDTO dto)
        {
            if(!this.ModelState.IsValid)
                return BadRequest();

            Users user = _mapper.Map<Users>(dto);
            user=await _service.SignUp(user);
            return Ok(new { user.Id, token = Utility.BuildToken(user, _config,"ADMIN") });
            
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> NewUser([FromBody] UserDTO dto)
        {
            if (!this.ModelState.IsValid)
                return BadRequest();
            Guid userId = Utility.GetCurrentUserID(User);

            Users user = _mapper.Map<Users>(dto);
            
            user = await _service.AddUser(user,userId,await _serviceService.GetByUserId(userId));
            if(user==null) return Forbid();
            return Ok(new { user.Id, token = Utility.BuildToken(user, _config,"DEVICE") });


        }

        [HttpPut("Profile")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> EditProfile([FromBody] UserDTO dto)
        {
            if (!this.ModelState.IsValid)
                return BadRequest();
            
            Guid userId = Utility.GetCurrentUserID(User);
            Users user = _mapper.Map<Users>(dto);
                        
            bool result = await _service.EditProfile(userId,user,Utility.HashPassword(dto.OldPassword,_config));
            if(result) return Ok();
            return StatusCode(403,new {result = "Wrong Password!"});


        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdateSubUsers(Guid id, [FromBody] UserDTO dto)
        {
            if (!this.ModelState.IsValid)
                return BadRequest();

            Guid parentId = Utility.GetCurrentUserID(User);
            Users user = _mapper.Map<Users>(dto);
            
            bool result=await  _service.UpdateSubUsers(id,parentId,user,await _serviceService.GetByUserId(parentId));
            if(result) return Ok();
            return StatusCode(403, new { result = "Invalid Access" });

        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public async Task<IActionResult> GetSubUsers()
        {
            Guid parentId = Utility.GetCurrentUserID(User);
            return Ok(_mapper.Map<UserDTO[]>(await _service.GetSubUsers(parentId))); 
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubUser(Guid id)
        {
            Guid parentId = Utility.GetCurrentUserID(User);
            return Ok(await _service.DeleteSubUser(id,parentId));

        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            Guid parentId = Utility.GetCurrentUserID(User);
            return Ok(_mapper.Map<UserDTO>(await _service.GetByIdAndParentId(id,parentId)));

        }

    }

   
}