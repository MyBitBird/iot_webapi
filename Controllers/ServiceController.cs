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
    [Route("api/Services")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        ServiceService _service;
        IMapper _mapper;

        public ServiceController(IOTContext context,IMapper mapper)
        {
            _service = new ServiceService(context);
            _mapper=mapper;
            
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> NewService([FromBody] ServiceDTO dto)
        {
            if(!this.ModelState.IsValid)
            {
                return BadRequest();
            }
            Guid userId = Utility.GetCurrentUserId(User);

            Models.Services newService = _mapper.Map<Models.Services>(dto);

           newService = await _service.NewService(newService,userId);

           return Ok(newService.Id);

        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdateService(Guid id,[FromBody] ServiceDTO dto)
        {
            if(!this.ModelState.IsValid)
                return BadRequest();

            Guid userId = Utility.GetCurrentUserId(User);
            Models.Services service = _mapper.Map<Models.Services>(dto);
            return Ok(await _service.UpdateService(id, service,userId));

        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(Guid id)
        {
            Guid userId = Utility.GetCurrentUserId(User);

            return Ok(await _service.Delete(id,userId));
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetMyServices()
        {
            Guid userId = Utility.GetCurrentUserId(User);
            return Ok(_mapper.Map<IList<ServiceDTO>>(await _service.GetByUserId(userId)));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetServiceById(Guid id)
        {
            Guid userId = Utility.GetCurrentUserId(User);
            return Ok(_mapper.Map<ServiceDTO>(await _service.GetById(id,userId)));

        }
    }
}