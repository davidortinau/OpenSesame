using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Flurl.Http;
using MyQNET.Enums;
using MyQNET.Models;

namespace MyQNET
{
    public class MyQService : IMyQService
    {
        string json = "";
        string securityToken = "";

        public bool IsAuthenticated
        {
            get { return !string.IsNullOrEmpty(securityToken); }
        }

        public void SetSecurityToken(string token)
        {
            this.securityToken = token;
        }

        public async Task<Tuple<bool, string>> Login(string username, string password)
        {
            try
            {
                var d = await $"{Endpoints.Login}"
                    .WithHeader("MyQApplicationId", Constants.APP_ID)
                    .PostJsonAsync(new
                    {
                        username = username,
                        password = password
                    })
                    .ReceiveString();

                var a = Newtonsoft.Json.Linq.JObject.Parse(d);
                securityToken = a["SecurityToken"].ToString();
            }
            catch (FlurlHttpTimeoutException)
            {
                //LogError("Request timed out.");
            }
            catch (FlurlHttpException ex)
            {
                //LogError(ex.Message);
            }

            return (new Tuple<bool, string>(!string.IsNullOrEmpty(securityToken), securityToken));
        }

        public async Task<List<Device>> GetDevices()
        {
            try
            {
                GetDevicesRootObject d = await $"{Endpoints.GetDevices}"
                    .WithHeaders(new
                    {
                        MyQApplicationId = Constants.APP_ID,
                        SecurityToken = securityToken
                    })
                    .GetJsonAsync<GetDevicesRootObject>();

                if (d.Devices == null)
                {
                    // need to login again
                    return null;
                }
                return d.Devices;
            }
            catch (FlurlHttpTimeoutException)
            {
                //LogError("Request timed out.");
                return null;
            }
            catch (FlurlHttpException ex)
            {
                //LogError(ex.Message);
                return null;
            }
        }

        async Task<int> GetDeviceState(int deviceId, string attributeName)
        {
            var response = $"{Endpoints.GetDeviceState}"
                .WithHeaders(new
                {
                    MyQApplicationId = Constants.APP_ID,
                    SecurityToken = securityToken
                })
                .SetQueryParams(new
                {
                    MyQDeviceId = deviceId,
                    AttributeName = attributeName
                })
                .GetStringAsync()
                .Result;

            var data = Newtonsoft.Json.Linq.JObject.Parse(response);
            return Int32.Parse(data["AttributeValue"].ToString(), NumberStyles.Integer);
        }

        public async Task<DoorState> GetDoorState(int deviceId)
        {
            var state = await GetDeviceState(deviceId, "doorstate");

            return (DoorState) state;
        }

        public async Task<bool> OpenDoor(int deviceId)
        {
            return await SetDeviceState(deviceId, "desireddoorstate", 1);
        }

        public async Task<bool> CloseDoor(int deviceId)
        {
            return await SetDeviceState(deviceId, "desireddoorstate", 0);
        }

        async Task<bool> SetDeviceState(int deviceId, string attribute, int state)
        {
            try
            {
                var response = await $"{Endpoints.SetDeviceState}"
                    .WithHeaders(new
                    {
                        MyQApplicationId = Constants.APP_ID,
                        SecurityToken = securityToken
                    })
                    .PutJsonAsync(new
                    {
                        MyQDeviceId = deviceId,
                        AttributeName = attribute,
                        AttributeValue = state
                    })
                    .ReceiveString();

                var data = Newtonsoft.Json.Linq.JObject.Parse(response);
            }
            catch (FlurlHttpTimeoutException)
            {
                //LogError("Request timed out.");
                return false;
            }
            catch (FlurlHttpException ex)
            {
                //LogError(ex.Message);
                return false;
            }

//			var data = Newtonsoft.Json.Linq.JObject.Parse(response);
//			return Int32.Parse(data["AttributeValue"].ToString(), NumberStyles.Integer);
            return true;
        }
    }
}