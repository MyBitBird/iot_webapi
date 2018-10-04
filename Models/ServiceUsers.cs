using System;
using System.Collections.Generic;

namespace IOT.Models
{
    public partial class ServiceUsers
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ServiceId { get; set; }
        public bool Deleted { get; set; }
        public DateTime RegisterDate { get; set; }

        public Services Service { get; set; }
        public Users User { get; set; }
    }
}
