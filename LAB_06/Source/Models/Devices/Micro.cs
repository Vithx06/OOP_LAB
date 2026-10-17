/****************/
// 202419017
// Đinh Thế Vinh
/****************/
using Source.Enum;

namespace Source.Models.Devices;

public class Micro : Device
{
    public Micro(string deviceId, string name, int maxBorrowDays)
    : base(deviceId, name, maxBorrowDays)
    {
    }
}