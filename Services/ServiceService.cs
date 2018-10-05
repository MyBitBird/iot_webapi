using System;
using System.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using IOT.Models;
using IOT.Helper;

namespace IOT.Services{

    public class ServiceService
    {
        IOTContext _context;

        public ServiceService(IOTContext context)
        {
            _context=context;

        }

        public Models.Services NewService(Models.Services service,Guid userId)
        {
            service.RegisterDate=DateTime.Now;
            service.Status=(short)MyEnums.ServiceStatus.ACTIVE;
            service.UserId=userId;
            _context.Services.Add(service);
            _context.SaveChanges();
            return service;
            
        }
    }

}