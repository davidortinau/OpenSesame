using System;
using System.Collections.Generic;

namespace MyQNET.Models
{
    public class Attribute
    {
        public int MyQDeviceTypeAttributeId { get; set; }
        public string Value { get; set; }
        public string UpdatedTime { get; set; }
        public bool IsDeviceProperty { get; set; }
        public string AttributeDisplayName { get; set; }
        public bool IsPersistent { get; set; }
        public bool IsTimeSeries { get; set; }
        public bool IsGlobal { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}