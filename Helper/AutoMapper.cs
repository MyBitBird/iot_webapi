using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using IOT.Models;
using IOT.DTO;
using Microsoft.Extensions.Configuration;


namespace IOT.Helper
{
    public class AutoMapper :  Profile
    {
        public AutoMapper(IConfiguration config)
        {
            CreateMap<ServiceDTO,Models.Services>().ReverseMap();
            CreateMap<ServicePropertiesDTO,ServiceProperties>().ReverseMap();
            CreateMap<UserDTO,Users>()
                .ForMember(d=>d.Password,
                           OperatingSystem=>OperatingSystem.MapFrom(s=>s.Password.Equals("") ?  "" : Utility.HashPassword(s.Password,config)));
            CreateMap<ServiceUsersDTO,ServiceUsers>().ReverseMap();
            CreateMap<ServiceLogDTO, ServiceLogs>().ForMember(d=>d.ServiceData,s=>s.Ignore());
        }
    }
}