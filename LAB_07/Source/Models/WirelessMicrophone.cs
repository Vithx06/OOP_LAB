/****************/
// 202419017
// Đinh Thế Vinh
/****************/
using System;
using Source.Enum;

namespace Source.Models.Devices;

public class WirelessMicrophone : TeachingDevice
{
    private int _batteryLevel;
    private int _signalQuality;

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

    public int SignalQuality
    {
        get => _signalQuality;
        private set
        {
            if (value < 0 || value > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(value),"Signal quality must be between 0 and 100.");
            }

            _signalQuality = value;
        }
    }

    public WirelessMicrophone(string id,string name,string storageLocation,int batteryLevel,int signalQuality)
    : base(id, name, storageLocation)
    {
        BatteryLevel = batteryLevel;
        SignalQuality = signalQuality;
    }

    // Điều kiện sẵn sàng của Micro: Sẵn có, Pin >=30%, Chất lượng tín hiệu >=40%
    public override bool IsReadyForUse()
    {
        return Status == DeviceStatus.Available && BatteryLevel >= 30 && SignalQuality >= 40;
    }

    // Điểm ưu tiên mặc định = (Pin + Chất lượng tín hiệu)/2
    // Để chia 2.0 do cần kq thực
    public override double GetDefaultPriorityScore()
    {
        return (BatteryLevel + SignalQuality) / 2.0;
    }
}