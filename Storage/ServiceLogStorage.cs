using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IOT.Models;
using Microsoft.EntityFrameworkCore;


namespace IOT.Storage
{
    public class ServiceLogStorage : IServiceLogStorage
    {
        private readonly IOTContext _context;
        public ServiceLogStorage(IOTContext context)
        {
            _context = context;
        }
        public async void AddAsync(ServiceLogs log)
        {
             await _context.ServiceLogs.AddAsync(log);
        }

        public async void AddServiceDataAsync(ServiceData data)
        {
            await _context.ServiceData.AddAsync(data);
        }

        public async void SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public IQueryable<ServiceLogs> JoinWithServiceDataAndProperty()
        {
            return _context.ServiceLogs.Include(s => s.ServiceData).ThenInclude(p => p.ServiceProperty);
        }

    }
}
