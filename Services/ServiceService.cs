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
            _context.SaveChanges();
            return service;
            
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
            _context.Services.Update(preService);
            _context.SaveChanges();
            return true;

        }

        public async Task<Boolean> Delete(Guid id,Guid userId)
        {
            Models.Services service = await  GetById(id,userId);

            if (service == null) return false;

            service.Status=(byte)MyEnums.ServiceStatus.DELETED;
            _context.Services.Update(service);
            _context.SaveChanges();
            return true;
        }

    }

}