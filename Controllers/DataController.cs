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
        private readonly ServiceLogService _service;
        private readonly ServiceService _servicesService;
        private readonly ServicePropertiesService _servicePropertiesService;
        private readonly IMapper _mapper;
        private readonly Guid _userId;

        public DataController(IOTContext context, IMapper mapper)
        {
            _service = new ServiceLogService(context);
            _servicesService = new ServiceService(context);
            _servicePropertiesService = new ServicePropertiesService(context);
            _mapper = mapper;
            _userId = Utility.GetCurrentUserId(User);

        }

        [HttpPost]
        [Authorize(Roles = "DEVICE")]
        public async Task<IActionResult> AddData([FromBody] ServiceLogDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (!await _servicesService.HaveUserAccess(_userId, dto.ServiceId))
                return Forbid();

            var log = _mapper.Map<ServiceLogs>(dto);
            log = await _service.AddData(_userId,
                                        log,
                                        dto.ServiceData,
                                        await _servicePropertiesService.GetValidPropertiesByServiceId(dto.ServiceId));
            return Ok(log.Id);
        }

        [HttpPost("{serviceId}")]
        [Authorize(Roles = "DEVICE")]
        public async Task<IActionResult> AddDataWithQuery(Guid serviceId, [FromQuery]DateTime logDate, string data)
        {
            if (IsDataValid(logDate, data))
                return BadRequest();

            if (!await _servicesService.HaveUserAccess(_userId, serviceId))
                return Forbid();

            var serviceData = DeserializeData(data);

            var logs = new ServiceLogs() { ServiceId = serviceId, LogDate = logDate };
            logs = await _service.AddData(_userId,
                                           logs,
                                           serviceData,
                                           await _servicePropertiesService.GetValidPropertiesByServiceId(serviceId));
            return Ok(new { logs.Id });

        }

        private static List<DeviceDataDTO> DeserializeData(string data)
        {
            var serviceData = new List<DeviceDataDTO>();
            var items = data.Split(',');

            const int keyValueLenght = 2;
            foreach (var item in items)
            {
                var value = item.Split(':');
                if (value.Length != keyValueLenght)
                    continue;
                serviceData.Add(new DeviceDataDTO() { Code = value[0], Data = value[1] });
            }

            return serviceData;
        }

        private static bool IsDataValid(DateTime logDate, string data)
        {
            return logDate < new DateTime(2000, 01, 01) || data == null || data.Equals("");
        }

        [HttpGet("{serviceId}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetData(Guid serviceId, [FromQuery] FilteringParams.Data filter)
        {
            var service = await _servicesService.GetById(serviceId, _userId);
            if (service == null)
                return Forbid();

            return Ok(_mapper.Map<ServiceLogDTO[]>(await _service.GetData(serviceId, filter)));

        }
    }

}