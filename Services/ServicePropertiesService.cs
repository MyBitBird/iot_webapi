using System;
using IOT.Models;
using System.Threading.Tasks;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace IOT.Services
{
    public class ServicePropertiesService
    {
        private readonly IOTContext _context;

        public ServicePropertiesService(IOTContext context)
        {
            _context = context;
        }

        public async Task<ServiceProperties[]> GetValidPropertiesByServiceId(Guid serviceId)
        {
            return await _context.ServiceProperties.Where(x => x.ServiceId == serviceId).ToArrayAsync();
        }

    }
}