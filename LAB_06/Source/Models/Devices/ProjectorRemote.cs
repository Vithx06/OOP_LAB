/****************/
// 202419017
// Đinh Thế Vinh
/****************/
using Source.Enum;

namespace Source.Models.Devices;

public class ProjectorRemote : Device
{
    public ProjectorRemote(string deviceId, string name, int maxBorrowDays)
    : base(deviceId, name, maxBorrowDays)
    {
    }
}