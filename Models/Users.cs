using System;
using System.Collections.Generic;
using IOT.Helper;

namespace IOT.Models
{
    public class Users
    {
        public Users()
        {
            //ServiceLogs = new HashSet<ServiceLogs>();
            //ServiceUsers = new HashSet<ServiceUsers>();
            //Services = new HashSet<Services>();
        }
        //TODO change type and status to enum

        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public MyEnums.UserStatus Status { get; set; }
        public MyEnums.UserTypes Type { get; set; }
        public DateTime RegisterDate { get; set; }
        public Guid? ParentUserId { get; set; }

        public ICollection<ServiceLogs> ServiceLogs { get; set; }
        public ICollection<ServiceUsers> ServiceUsers { get; set; }
        public ICollection<Services> Services { get; set; }
    }
}
