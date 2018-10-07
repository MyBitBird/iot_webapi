using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using IOT.Models;
using IOT.DTO;


namespace IOT.Helper
{
    public class AutoMapper :  Profile
    {
        public AutoMapper()
        {
            CreateMap<ServiceDTO,Models.Services>().ReverseMap();
        }
    }
}