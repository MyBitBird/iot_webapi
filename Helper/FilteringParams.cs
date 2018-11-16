using System;
using System.Data;

namespace IOT.Helper
{
    public class FilteringParams 
    {
        public class Data {
            public Guid? userId { get; set; } = null;
            public int limit {get; set;} = 1000;
            public int offset { get; set; } = 0;
            public string sort { get; set; } = "";
            public DateTime? from { get; set; } = null;
            public DateTime? to { get; set; } = null;
        }
    }
}