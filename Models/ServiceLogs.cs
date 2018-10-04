using System;
using System.Collections.Generic;

namespace IOT.Models
{
    public partial class ServiceLogs
    {
        public Guid Id { get; set; }
        public Guid ServiceId { get; set; }
        public Guid UserId { get; set; }
        public DateTime LogDate { get; set; }
        public DateTime RegisterDate { get; set; }

        public Services Service { get; set; }
        public Users User { get; set; }
    }
}
