using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyQNET.Enums;
using MyQNET.Models;

namespace MyQNET
{
    public interface IMyQService
    {
        bool IsAuthenticated { get; }
        Task<List<Device>> GetDevices();
        Task<Tuple<bool, string>> Login(string username, string password);
        Task<DoorState> GetDoorState(int deviceId);
        Task<bool> OpenDoor(int deviceId);
        Task<bool> CloseDoor(int deviceId);
        void SetSecurityToken(string securityToken);
    }
}