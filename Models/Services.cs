using System;
using System.Collections.Generic;

namespace IOT.Models
{
    public class Services
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public short Status { get; set; }
        public DateTime RegisterDate { get; set; }

        public Users User { get; set; }
        public ICollection<ServiceLogs> ServiceLogs { get; set; }
        public ICollection<ServiceProperties> ServiceProperties { get; set; }
        public ICollection<ServiceUsers> ServiceUsers { get; set; }
    }
}
