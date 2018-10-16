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
    [Route("api/Data")]
    [ApiController]

    public class DataController : ControllerBase
    {
        ServiceLogService _service;
        ServiceService _servicesService;
        ServicePropertiesService _servicePropertiesService;
        IMapper _mapper;

        public DataController(IOTContext context, IMapper mapper)
        {
            _service = new ServiceLogService(context);
            _servicesService = new ServiceService(context);
            _servicePropertiesService = new ServicePropertiesService(context);
            _mapper = mapper;

        }

        [HttpPost,Authorize]
        public async Task<IActionResult> AddData([FromBody] ServiceLogDTO dto)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest();
            }

            Guid userId = Utility.GetCurrentUserID(User); 
            if (!await _servicesService.HaveUserAccess(userId, dto.ServiceId)) return Forbid();

            ServiceLogs log = _mapper.Map<ServiceLogs>(dto);

            return Ok(new { _service.AddData(userId,log,dto.ServiceData,await _servicePropertiesService.GetValidPropertiesByServiceId(dto.ServiceId)).Id});
            
        }
    }

}