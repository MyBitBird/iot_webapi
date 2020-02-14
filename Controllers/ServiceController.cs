using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IOT.DTO;
using Microsoft.AspNetCore.Authorization;
using IOT.Models;
using IOT.Services;
using IOT.Helper;
using AutoMapper;


namespace IOT.Controllers
{
    [Produces("application/json")]
    [Route("api/Services")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly ServiceService _service;
        private readonly IMapper _mapper;
        
        public ServiceController(IOTContext context, IMapper mapper)
        {
            _service = new ServiceService(context);
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Add([FromBody] ServiceDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var userId = Utility.GetCurrentUserId(User);

            var newService = _mapper.Map<Models.Services>(dto);

            newService = await _service.NewService(newService, userId);

            return Ok(newService.Id);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ServiceDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var userId = Utility.GetCurrentUserId(User);

            var service = _mapper.Map<Models.Services>(dto);
            return Ok(await _service.UpdateService(id, service, userId));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = Utility.GetCurrentUserId(User);
            return Ok(await _service.Delete(id, userId));
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetMyServices()
        {
            var userId = Utility.GetCurrentUserId(User);
            return Ok(_mapper.Map<IList<ServiceDTO>>(await _service.GetByUserId(userId)));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetServiceById(Guid id)
        {
            var userId = Utility.GetCurrentUserId(User);
            return Ok(_mapper.Map<ServiceDTO>(await _service.GetById(id, userId)));

        }
    }
}