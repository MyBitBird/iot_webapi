using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IOT.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace IOT.Storage
{
    public class ServiceLogStorage : IServiceLogStorage
    {
        private readonly IOTContext _context;
        public ServiceLogStorage(IOTContext context)
        {
            _context = context;
        }
        public async Task<EntityEntry<ServiceLogs>> AddAsync(ServiceLogs log)
        {
            return await _context.ServiceLogs.AddAsync(log);
        }

        public async Task<EntityEntry<ServiceData>> AddServiceDataAsync(ServiceData data)
        {
            return await _context.ServiceData.AddAsync(data);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public IQueryable<ServiceLogs> AsQueryable()
        {
            return _context.ServiceLogs.AsQueryable();
        }
    }
}
