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
            switch(filters.Sort)
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
                && (filters.UserId == null || x.UserId == filters.UserId )
                && (filters.From == null || x.LogDate >= filters.From)
                && (filters.To == null || x.LogDate <= filters.To)

            ).Skip(filters.Offset).Take(filters.Limit).ToArrayAsync();
        }

    }

}