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

            Users login_user= _service.Authenticate(dto.Username,dto.Password);
            if(login_user ==null) return Unauthorized();

            return Ok(new {login_user.Id,token= Utility.BuildToken(login_user,_config) });

        }

        [HttpPost("SignUp"),AllowAnonymous]
        public async Task<IActionResult> SignUp([FromBody] UserDTO dto)
        {
            if(!this.ModelState.IsValid)
                return BadRequest();

            Users user = _mapper.Map<Users>(dto);
            user=await _service.SignUp(user);
            return Ok(new { user.Id, token = Utility.BuildToken(user, _config) });
            
        }

        [HttpPost,Authorize]
        public async Task<IActionResult> NewUser([FromBody] UserDTO dto)
        {
            if (!this.ModelState.IsValid)
                return BadRequest();

            Users user = _mapper.Map<Users>(dto);
            
            user = await _service.AddUser(user,await _serviceService.GetByUserId(Utility.GetCurrentUserID(User)));
            if(user==null) return Forbid();
            return Ok(new { user.Id, token = Utility.BuildToken(user, _config) });
            


        }
        


    }
}