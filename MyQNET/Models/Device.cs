using System;
using System.Collections.Generic;

namespace MyQNET.Models
{
    public class Device
    {
        public int MyQDeviceId { get; set; }
        public int ParentMyQDeviceId { get; set; }
        public int MyQDeviceTypeId { get; set; }
        public string MyQDeviceTypeName { get; set; }
        public DateTime RegistrationDateTime { get; set; }
        public string SerialNumber { get; set; }
        public string UserName { get; set; }
        public int UserCountryId { get; set; }
        public List<Attribute> Attributes { get; set; }
        public string ChildrenMyQDeviceIds { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string ConnectServerDeviceId { get; set; }
    }

    public class GetDevicesRootObject
    {
        public List<Device> Devices { get; set; }
        public string ReturnCode { get; set; }
        public string ErrorMessage { get; set; }
        public string CorrelationId { get; set; }
    }
}