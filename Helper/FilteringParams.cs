using System;
using System.Data;

namespace IOT.Helper
{
    public class FilteringParams 
    {
        public class Data {
            public Guid? UserId { get; set; } = null;
            public int Limit {get; set;} = 1000;
            public int Offset { get; set; } = 0;
            public string Sort { get; set; } = "";
            public DateTime? From { get; set; } = null;
            public DateTime? To { get; set; } = null;
        }
    }
}