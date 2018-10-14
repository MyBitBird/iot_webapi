using System;
using System.Collections.Generic;

namespace IOT.Models
{
    public partial class Users
    {
        public Users()
        {
            ServiceLogs = new HashSet<ServiceLogs>();
            ServiceUsers = new HashSet<ServiceUsers>();
            Services = new HashSet<Services>();
        }

        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public short Status { get; set; }
        public short Type { get; set; }
        public DateTime RegisterDate { get; set; }
        public Guid? ParentUserId { get; set; }

        public ICollection<ServiceLogs> ServiceLogs { get; set; }
        public ICollection<ServiceUsers> ServiceUsers { get; set; }
        public ICollection<Services> Services { get; set; }
    }
}
