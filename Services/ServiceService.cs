using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using IOT.Models;
using IOT.Helper;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace IOT.Services
{

    public class ServiceService
    {
        private readonly IOTContext _context;

        public ServiceService(IOTContext context)
        {
            _context = context;
        }
        public async Task<Models.Services> Add(Models.Services service, Guid userId)
        {
            service.RegisterDate = DateTime.Now;
            service.Status = MyEnums.ServiceStatus.Active;
            service.UserId = userId;
            await _context.Services.AddAsync(service);
            await _context.SaveChangesAsync();
            return service;
        }

        public async Task<bool> HaveUserAccess(Guid userId, Guid serviceId)
        {
            return await _context.ServiceUsers.AnyAsync(x => x.ServiceId == serviceId && x.UserId == userId && x.Deleted == false);
        }

        public async Task<Models.Services> GetById(Guid id, Guid userId)
        {
            var service = await _context.Services.AsNoTracking()
                .Include(i => i.ServiceProperties)
                .FirstOrDefaultAsync(x => x.Id == id &&
                                          x.UserId == userId &&
                                          x.Status == MyEnums.ServiceStatus.Active);

            service.ServiceProperties = service.ServiceProperties.Where(x => x.Deleted == false).ToArray();
            return service;
        }

        public async Task<Models.Services[]> GetByUserId(Guid userId)
        {
            return await _context.Services.Where(x => x.UserId == userId && x.Status == MyEnums.ServiceStatus.Active).ToArrayAsync();
        }

        public async Task<bool> UpdateService(Guid id, Models.Services service, Guid userId)
        {
            var preService = await GetById(id, userId);
            
            var newProperties = UpdateExProperties(service, preService);
            AddNewPropertiesToService(id, newProperties);

            preService.Title = service.Title;
            _context.Services.Update(preService);
            await _context.SaveChangesAsync();
            return true;

        }
        private IEnumerable<ServiceProperties> UpdateExProperties(Models.Services newService, Models.Services exService)
        {
            //replacing newService old properties with new ones
            var newProperties = newService.ServiceProperties.ToList();
            var oldProperties = exService.ServiceProperties.ToList();
            exService.ServiceProperties = null;

            foreach (var property in oldProperties)
            {
                var isPropertyExist = newService.ServiceProperties.FirstOrDefault(x => x.Id == property.Id);
                if (isPropertyExist != null) //update old properties info
                {
                    property.Title = isPropertyExist.Title;
                    property.Code = isPropertyExist.Code;
                    newProperties.Remove(isPropertyExist);
                }
                else //remove oldProperties that doesnt exist in new properties
                    property.Deleted = true;

                _context.ServiceProperties.Update(property);
            }

            return newProperties;
        }

        private void AddNewPropertiesToService(Guid serviceId, IEnumerable<ServiceProperties> newProperties)
        {
            foreach (var property in newProperties)
            {
                property.ServiceId = serviceId;
                _context.ServiceProperties.Add(property);
            }
        }

        public async Task<bool> Delete(Guid id, Guid userId)
        {
            var service = await GetById(id, userId);

            if (service == null) return false;

            service.Status = MyEnums.ServiceStatus.Deleted;
            _context.Services.Update(service);
            await _context.SaveChangesAsync();
            return true;
        }

    }

}