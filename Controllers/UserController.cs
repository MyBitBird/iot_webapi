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


namespace IOT.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        UserService _service;
        IConfiguration _config;

        public UserController(IOTContext context,IConfiguration config)
        {
            _service = new UserService(context);
            _config=config;


        }

        [HttpPost("Authenticate")]
        [AllowAnonymous]
        public IActionResult Authenticate([FromBody] AuthorizeDTO dto )
        {
            if(dto==null)
                return BadRequest();
            
            Users login_user= _service.Authenticate(dto.Username,dto.Password);
            if(login_user ==null) return Unauthorized();

            return Ok(new {login_user.Id,token= Utility.BuildToken(login_user,_config) });

        }


    }
}