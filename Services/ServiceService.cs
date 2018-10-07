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
            _context.Services.Add(service);
            _context.SaveChanges();
            return service;
            
        }

        public async Task<Models.Services> GetById(Guid id)
        {
            return await _context.Services.FirstOrDefaultAsync(x=>x.Id==id);
        }

        public async Task<Models.Services[]> GetByUserId(Guid userId)
        {
            return await _context.Services.Where(x => x.UserId == userId).ToArrayAsync();
        }

        public async Task<Boolean> UpdateService(Guid id, Models.Services service)
        {
            Models.Services preService = await GetById(id);
            preService.Title = service.Title;
            _context.Services.Update(preService);
            _context.SaveChanges();
            return true;


        }

        public async Task<Boolean> Delete(Guid id)
        {
            Models.Services service = await  GetById(id);
            service.Status=(byte)MyEnums.ServiceStatus.DELETED;
            _context.Services.Update(service);
            _context.SaveChanges();
            return true;
        }

    }

}