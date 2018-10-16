using System;
using System.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using IOT.Models;
using IOT.Helper;
using System.Threading.Tasks;

namespace IOT.Services{

    public class ServiceService
    {
        IOTContext _context;

        public ServiceService(IOTContext context)
        {
            _context=context;

        }
        public async Task<Models.Services> NewService(Models.Services service,Guid userId)
        {
            service.RegisterDate=DateTime.Now;
            service.Status=(short)MyEnums.ServiceStatus.ACTIVE;
            service.UserId=userId;
            await _context.Services.AddAsync(service);
            await _context.SaveChangesAsync();
            return service;
            
        }

        public async Task<bool> HaveUserAccess(Guid userId,Guid serviceId)
        {
            return await _context.ServiceUsers.AnyAsync(x=>x.ServiceId==serviceId && x.UserId==userId && x.Deleted==false);
        }

        public async Task<Models.Services> GetById(Guid id,Guid userId)
        {
            return await _context.Services.Include(i=>i.ServiceProperties).FirstOrDefaultAsync(x=>x.Id==id && x.UserId==userId);
        }

        public async Task<Models.Services[]> GetByUserId(Guid userId)
        {
            return await _context.Services.Where(x => x.UserId == userId).ToArrayAsync();
        }

        public async Task<Boolean> UpdateService(Guid id, Models.Services service,Guid userId)
        {
            Models.Services preService = await GetById(id,userId);
            
            if(preService==null) return false;

            preService.Title = service.Title;

            foreach(ServiceProperties property in preService.ServiceProperties)
            {
                if(service.ServiceProperties.Any(x=>x.Code==property.Code)) continue;
                else 
                {
                    property.Deleted=true;
                    _context.ServiceProperties.Update(property);
                }
            }

            foreach (ServiceProperties property in service.ServiceProperties)
            {
                if (preService.ServiceProperties.Any(x => x.Code == property.Code)) continue;
                else
                {
                    property.ServiceId=id;                    
                    await _context.ServiceProperties.AddAsync(property);
                }
            }
            _context.Services.Update(preService);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<Boolean> Delete(Guid id,Guid userId)
        {
            Models.Services service = await  GetById(id,userId);

            if (service == null) return false;

            service.Status=(byte)MyEnums.ServiceStatus.DELETED;
            _context.Services.Update(service);
            await _context.SaveChangesAsync();
            return true;
        }

    }

}