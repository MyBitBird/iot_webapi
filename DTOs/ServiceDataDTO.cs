using System;

namespace IOT.DTO
{
    public class ServiceDataDTO 
    {
        public Guid Id { get; set; }
        public Guid ServicePropertyId { get; set; }
        public string Data { get; set; }

    }
}