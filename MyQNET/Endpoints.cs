namespace MyQNET
{
    public static class Endpoints
    {
        const string BASE = "https://myqexternal.myqdevice.com";

        public static string Login => $"{BASE}/api/v4/User/Validate";
        public static string GetDevices => $"{BASE}/api/v4/userdevicedetails/get";
        public static string GetDeviceState => $"{BASE}/api/v4/deviceattribute/getdeviceattribute";
        public static string SetDeviceState => $"{BASE}/api/v4/deviceattribute/putdeviceattribute";
    }
}