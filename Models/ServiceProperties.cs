using System;
using System.Collections.Generic;

namespace IOT.Models
{
    public class ServiceProperties
    {
        public Guid Id { get; set; }
        public Guid ServiceId { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public bool Deleted { get; set; }

        public Services Service { get; set; }
        public ICollection<ServiceData> ServiceData { get; set; }
    }
}
