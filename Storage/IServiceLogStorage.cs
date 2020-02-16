using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IOT.Models;

namespace IOT.Storage
{
    public interface IServiceLogStorage
    {
        void AddAsync(ServiceLogs log);

        void AddServiceDataAsync(ServiceData data);

        void SaveChangesAsync();

        IQueryable<ServiceLogs> JoinWithServiceDataAndProperty();

    }
}