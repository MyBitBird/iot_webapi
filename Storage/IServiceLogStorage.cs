using System.Linq;
using System.Threading.Tasks;
using IOT.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace IOT.Storage
{
    public interface IServiceLogStorage
    {
        Task<EntityEntry<ServiceLogs>> AddAsync(ServiceLogs log);

        Task<EntityEntry<ServiceData>> AddServiceDataAsync(ServiceData data);

        Task<int> SaveChangesAsync();

        IQueryable<ServiceLogs> AsQueryable();

    }
}