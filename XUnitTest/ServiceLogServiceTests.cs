using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IOT.DTO;
using IOT.Helper;
using IOT.Models;
using IOT.Services;
using IOT.Storage;
using Moq;
using Xunit;


namespace XUnitTest
{
    public class ServiceLogServiceTests
    {
        private readonly Mock<IServiceLogStorage> _serviceLogStorage;
        private readonly ServiceLogService _service;
        public ServiceLogServiceTests()
        {
            _serviceLogStorage = new Mock<IServiceLogStorage>();
            _service = new ServiceLogService(_serviceLogStorage.Object);

        }

        [Fact]
        public void AddData_NullUserId_ThrowArgumentException()
        {
            var log = new ServiceLogs { Id = new Guid() };
            Assert.Throws<ArgumentException>(() => _service.AddData(log, null, null));
        }

        [Fact]
        public void AddData_NullLog_ThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _service.AddData(null, null, null));
        }

        [Fact]
        public void AddData_WhenCalled_CallAddSync()
        {
            var log = new ServiceLogs { UserId = Guid.NewGuid() };
            _service.AddData(log, new List<DeviceDataDTO>(), new ServiceProperties[] { });
            _serviceLogStorage.Verify(x => x.AddAsync(log), Times.Once);
        }

        [Fact]
        public void AddData_WhenCalled_CallSaveServiceData()
        {
            var log = new ServiceLogs { Id = Guid.NewGuid(), UserId = Guid.NewGuid() };
            _service.AddData(log, new List<DeviceDataDTO>(), new ServiceProperties[] { });
            _serviceLogStorage.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000", 0)]
        [InlineData("00000000-0000-0000-0000-0000000000A1", 2)]
        [InlineData("00000000-0000-0000-0000-0000000000A2", 1)]
        public void GetData_FilterByServiceId_ReturnFilteredResult(string serviceId, int result)
        {
            SetupForGetData();
            var filterData = new FilteringParams.Data { UserId = null };
            var res = _service.GetData(Guid.Parse(serviceId), filterData);
            Assert.Equal(result, res.Length);
        }

        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000", 0)]
        [InlineData("00000000-0000-0000-0000-0000000000B1", 1)]
        [InlineData("00000000-0000-0000-0000-0000000000B3", 0)]
        public void GetData_FilterByUserId_ReturnFilteredResult(string userId, int result)
        {
            SetupForGetData();
            var filterData = new FilteringParams.Data { UserId = Guid.Parse(userId) };
            var res = _service.GetData(Guid.Parse("00000000-0000-0000-0000-0000000000A1"), filterData);
            Assert.Equal(result, res.Length);
        }

        [Theory]
        [InlineData("2020-01-01", "2020-01-01", 0)]
        [InlineData("2020-01-01", "2020-01-02", 1)]
        [InlineData("2020-01-01", "2020-01-04", 2)]
        [InlineData("2020-01-02", "2020-01-01", 0)]
        public void GetData_FilterByLogDate_ReturnFilteredResult(string from, string to, int result)
        {
            SetupForGetData();
            var filterData = new FilteringParams.Data { UserId = null, From = DateTime.Parse(from), To = DateTime.Parse(to) };
            var res = _service.GetData(Guid.Parse("00000000-0000-0000-0000-0000000000A1"), filterData);
            Assert.Equal(result, res.Length);
        }

        private void SetupForGetData()
        {
            var query = InitSampleData();
            _serviceLogStorage.Setup(x => x.JoinWithServiceDataAndProperty()).Returns(query.AsQueryable());
        }


        private static IEnumerable<ServiceLogs> InitSampleData()
        {
            var query = new List<ServiceLogs>
            {
                new ServiceLogs
                {
                    ServiceId = Guid.Parse("00000000-0000-0000-0000-0000000000A1"),
                    UserId = Guid.Parse("00000000-0000-0000-0000-0000000000B1"),
                    LogDate = new DateTime(2020,01,02)
                },
                new ServiceLogs
                {
                    ServiceId = Guid.Parse("00000000-0000-0000-0000-0000000000A2"),
                    UserId = Guid.Parse("00000000-0000-0000-0000-0000000000B1"),
                    LogDate = new DateTime(2020,01,03)
                },
                new ServiceLogs
                {
                    ServiceId = Guid.Parse("00000000-0000-0000-0000-0000000000A1"),
                    UserId = Guid.Parse("00000000-0000-0000-0000-0000000000B2"),
                    LogDate = new DateTime(2020,01,04)
                },
                new ServiceLogs
                {
                    ServiceId = Guid.Parse("00000000-0000-0000-0000-0000000000A3"),
                    UserId = Guid.Parse("00000000-0000-0000-0000-0000000000B3"),
                    LogDate = new DateTime(2020,01,05)
                }
            };
            return query;
        }

    }
}
