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

    public override decimal CalculateLateFee(int lateDays)
    {
        if (lateDays < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(lateDays), "Late days cannot be negative.");
        }

        return lateDays * 20000m;
    }
}