using System;
using IOT.Models;
using IOT.DTO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using IOT.Helper;
using Microsoft.EntityFrameworkCore;

namespace IOT.Services
{

    public class ServiceLogService
    {
        IOTContext _context;

        public ServiceLogService(IOTContext context)
        {
            _context=context;
        }

        public async  Task<ServiceLogs> AddData(Guid userId, ServiceLogs log,ICollection<DeviceDataDTO> data,ServiceProperties[] validProperties)
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
            return  log;

        }

        public async Task<ServiceLogs[]> GetData(Guid serviceId, FilteringParams.Data filters)
        {
            var query = _context.ServiceLogs.AsQueryable();
            switch(filters.sort)
            {
                case "logDate":
                    query = query.OrderBy(x=>x.LogDate);
                    break;

                case "-logDate":
                    query = query.OrderByDescending(x => x.LogDate);
                    break;

                case "registerDate":
                    query = query.OrderBy(x => x.RegisterDate);
                    break;
                
                case "-registerDate":
                    query = query.OrderByDescending(x => x.RegisterDate);
                    break;
            }

            return await query.Include(x => x.ServiceData).ThenInclude(y => y.ServiceProperty).Where
            (x => x.ServiceId == serviceId 
                && (filters.userId == null || x.UserId == filters.userId )
                && (filters.from == null || x.LogDate >= filters.from)
                && (filters.to == null || x.LogDate <= filters.to)

            ).Skip(filters.offset).Take(filters.limit).ToArrayAsync();
        }

    }

}