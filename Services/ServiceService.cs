using System;
using System.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using IOT.Models;
using IOT.Helper;
using System.Threading.Tasks;
using System.Collections.Generic;

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
            service.Status=MyEnums.ServiceStatus.Active;
            service.UserId=userId;
            await _context.Services.AddAsync(service);
            await _context.SaveChangesAsync();
            return service;
            
        }

        public async Task<bool> HaveUserAccess(Guid userId,Guid serviceId)
        {
            return await _context.ServiceUsers.AnyAsync(x=>x.ServiceId == serviceId && x.UserId == userId && x.Deleted == false);
        }

        public async Task<Models.Services> GetById(Guid id,Guid userId)
        {
            Models.Services service= await _context.Services.AsNoTracking().Include(i=>i.ServiceProperties).FirstOrDefaultAsync(x=>x.Id==id && x.UserId==userId && x.Status == MyEnums.ServiceStatus.Active);
            service.ServiceProperties= service.ServiceProperties.Where(x=>x.Deleted==false).ToArray();
            return service;
        }

        public async Task<Models.Services[]> GetByUserId(Guid userId)
        {
            return await _context.Services.Where(x => x.UserId == userId && x.Status == MyEnums.ServiceStatus.Active).ToArrayAsync();
        }

        public async Task<Boolean> UpdateService(Guid id, Models.Services service,Guid userId)
        {
            Models.Services preService = await GetById(id,userId);
            
            if(preService==null) return false;

            preService.Title = service.Title;

            List<ServiceProperties> newProperties = service.ServiceProperties.ToList();

            List<ServiceProperties> oldProperties = preService.ServiceProperties.ToList();

            preService.ServiceProperties = null;//becuase we will see stupid error in add new item : exception collection was of a fixed size entity framework

            foreach(ServiceProperties property in oldProperties)  
            {
                ServiceProperties isPropertyExist = service.ServiceProperties.FirstOrDefault(x => x.Id == property.Id);
                if(isPropertyExist!=null)
                {
                    property.Title = isPropertyExist.Title;
                    property.Code = isPropertyExist.Code;
                    newProperties.Remove(isPropertyExist);
                } 
                else 
                    property.Deleted=true;
                    
               _context.ServiceProperties.Update(property);
            }

            _context.Services.Update(preService);

           foreach (ServiceProperties property in newProperties)
            {
                property.ServiceId = id;
                _context.ServiceProperties.Add(property);
              
            }
            
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<Boolean> Delete(Guid id,Guid userId)
        {
            Models.Services service = await  GetById(id,userId);

            if (service == null) return false;

            service.Status=MyEnums.ServiceStatus.Deleted;
            _context.Services.Update(service);
            await _context.SaveChangesAsync();
            return true;
        }

    }

}