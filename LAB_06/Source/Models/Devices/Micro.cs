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

    public override decimal CalculateLateFee(int lateDays)
    {
        if (lateDays < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(lateDays), "Late days cannot be negative.");
        }

        return lateDays * 30000m;
    }
}