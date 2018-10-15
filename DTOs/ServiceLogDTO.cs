using System;
using IOT.DTO;
using System.Collections.Generic;

namespace IOT.DTO
{
    public class ServiceLogDTO
    {
        public Guid ServiceId { get; set; }
        public DateTime LogDate { get; set; }
        public DateTime RegisterDate { get; set; }
        public ICollection<ServiceDataDTO> ServiceData { get; set; }

    }
}