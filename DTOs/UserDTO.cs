using System;
using System.Collections.Generic;
using IOT.DTO;


namespace IOT.DTO
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }

        public ICollection<ServiceUsersDTO> ServiceUsers { get; set; }
       
    }
    
}