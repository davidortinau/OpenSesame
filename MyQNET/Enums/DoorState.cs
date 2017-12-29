using System.ComponentModel;

namespace MyQNET.Enums
{
    public enum DoorState
    {
        [Description("Unknown")] Unknown = 0,

        [Description("Open")] Open = 1,

        [Description("Closed")] Closed = 2,

        [Description("Stopped in the Middle")] StoppedInMiddle = 3,

        [Description("Going Up")] GoingUp = 4,

        [Description("Going Down")] GoingDown = 5,

        [Description("Not Closed")] NotClosed = 9
    }
}