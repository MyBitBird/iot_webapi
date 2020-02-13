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
                           operatingSystem=>operatingSystem.MapFrom(s=>s.Password.Equals("") ?  "" : Utility.HashPassword(s.Password,config)));
            
            CreateMap<Users,UserDTO>().ForMember(d => d.Password,s=>s.UseValue(""));
            
            CreateMap<ServiceUsersDTO,ServiceUsers>().ReverseMap();
            
            CreateMap<ServiceLogDTO, ServiceLogs>().ForMember(d=>d.ServiceData,s=>s.Ignore());

            CreateMap<ServiceLogs, ServiceLogDTO>();
            
            CreateMap<ServiceData,DeviceDataDTO>().ForMember(x => x.Code, operatingSystem => operatingSystem.MapFrom(s => s.ServiceProperty.Code));
        }
    }
}