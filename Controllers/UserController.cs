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
            
            Users tmp= _service.Authenticate(dto.Username,dto.Password);
            if(tmp==null) return Unauthorized();

            return Ok(new {tmp.Id,token= Utility.BuildToken(tmp,_config) });

        }


    }
}