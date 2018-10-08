using System;
using System.Collections.Generic;

namespace IOT.DTO
{
    public class ServiceDTO
    {
        public string title {set;get;}

        public ICollection<ServicePropertiesDTO> ServiceProperties { get; set; }


    }
}