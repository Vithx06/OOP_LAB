/****************/
// 202419017
// Đinh Thế Vinh
/****************/
using Source.Enum;

namespace Source.Models.Devices;

public class ProjectorRemote : TeachingDevice
{
    private int _batteryLevel;

    public int BatteryLevel
    {
        get => _batteryLevel;
        private set
        {
            if (value < 0 || value > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(value),"Battery level must be between 0 and 100.");
            }

            _batteryLevel = value;
        }
    }

    public ProjectorRemote(string id,string name,string storageLocation,int batteryLevel)
    : base(id, name, storageLocation)
    {
        BatteryLevel = batteryLevel;
    }

    // Điều khiển máy chiếu tương thích với tất cả các phòng
    public override bool IsCompatibleWith(string roomId)
    {
        return !string.IsNullOrWhiteSpace(roomId);
    }

    // Điều kiện sẵn sàng của Đk máy chiếu: Sẵn có, Pin >=20%
    public override bool IsReadyForUse()
    {
        return Status == DeviceStatus.Available && BatteryLevel >= 20;
    }

    // Điểm ưu tiên mặc định = Pin
    public override double GetDefaultPriorityScore(string roomId)
    {
        return BatteryLevel;
    }
}