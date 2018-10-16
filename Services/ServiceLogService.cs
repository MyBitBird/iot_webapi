using System;
using IOT.Models;
using IOT.DTO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace IOT.Services
{

    public class ServiceLogService
    {
        IOTContext _context;

        public ServiceLogService(IOTContext context)
        {
            _context=context;
        }

        public async Task<ServiceLogs> AddData(Guid userId, ServiceLogs log,ICollection<ServiceDataDTO> data,ServiceProperties[] validProperties)
        {
            log.UserId=userId;
            log.RegisterDate=DateTime.Now;
            await _context.ServiceLogs.AddAsync(log);
            foreach(var item in data )
            {
                ServiceProperties prop=validProperties.FirstOrDefault(x=>x.Code.ToLower() ==item.Code.ToLower());
                if(prop==null) continue;
                await _context.ServiceData.AddAsync(new ServiceData(){Data= item.Data,ServiceLogId=log.Id,ServicePropertyId=prop.Id});
            }
            await _context.SaveChangesAsync();
            return log;

        }

    }

}