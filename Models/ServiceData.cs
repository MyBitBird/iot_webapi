using System;
using System.Collections.Generic;

namespace IOT.Models
{
    public class ServiceData
    {
        public Guid Id { get; set; }
        public Guid ServicePropertyId { get; set; }
        public string Data { get; set; }
        public Guid ServiceLogId { get; set; }

        public ServiceLogs ServiceLog { get; set; }
        public ServiceProperties ServiceProperty { get; set; }
    }
}
