using System;
using IOT.Models;
using IOT.DTO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using IOT.Helper;
using IOT.Storage;
using Microsoft.EntityFrameworkCore;

namespace IOT.Services
{
    public class DataWithProperties
    {
        public DeviceDataDTO DataDto { get; set; }
        public ServiceProperties Properties { get; set; }
    }

    public class ServiceLogService
    {
        private readonly IServiceLogStorage _serviceLogStorage;

        public ServiceLogService(IServiceLogStorage serviceLogStorage)
        {
            _serviceLogStorage = serviceLogStorage;
        }

        public ServiceLogs AddData(ServiceLogs log, ICollection<DeviceDataDTO> data,
            ServiceProperties[] validProperties)
        {
            if (log == null || log.UserId == Guid.Empty)
                throw new ArgumentException();

            log.RegisterDate = DateTime.Now;

            _serviceLogStorage.AddAsync(log);

            var dataWithRelatedProperty = FilterDataWithValidProperty(data, validProperties);

            SaveServiceData(log.Id, dataWithRelatedProperty);

            _serviceLogStorage.SaveChangesAsync();
            return log;
        }

        private void SaveServiceData(Guid serviceLogId, IEnumerable<DataWithProperties> dataWithProperties)
        {
            foreach (var obj in dataWithProperties)
            {
                _serviceLogStorage.AddServiceDataAsync(new ServiceData()
                {
                    Data = obj.DataDto.Data,
                    ServiceLogId = serviceLogId,
                    ServicePropertyId = obj.Properties.Id
                });
            }
        }

        private static IEnumerable<DataWithProperties> FilterDataWithValidProperty(ICollection<DeviceDataDTO> data, ServiceProperties[] validProperties)
        {
            var filteredData = validProperties.Join(data, v => v.Code.ToLower(), d => d.Code.ToLower(),
                (v, d) => new DataWithProperties() { DataDto = d, Properties = v });
            return filteredData;
        }

        public ServiceLogs[] GetData(Guid serviceId, FilteringParams.Data filters)
        {
            var query = _serviceLogStorage.JoinWithServiceDataAndProperty();

            query = GetQueryOrder(query, filters.Sort);

            return query.Where(
                x => x.ServiceId == serviceId
                     && (filters.UserId == null || x.UserId == filters.UserId)
                     && (filters.From == null || x.LogDate >= filters.From)
                     && (filters.To == null || x.LogDate <= filters.To)

            ).Skip(filters.Offset).Take(filters.Limit).ToArray();
        }

        private IQueryable<ServiceLogs> GetQueryOrder(IQueryable<ServiceLogs> query, string sortBy)
        {
            switch (sortBy)
            {
                case "logDate":
                    query = query.OrderBy(x => x.LogDate);
                    break;

                case "-logDate":
                    query = query.OrderByDescending(x => x.LogDate);
                    break;

                case "registerDate":
                    query = query.OrderBy(x => x.RegisterDate);
                    break;

                case "-registerDate":
                    query = query.OrderByDescending(x => x.RegisterDate);
                    break;
            }
            return query;
        }
    }
}