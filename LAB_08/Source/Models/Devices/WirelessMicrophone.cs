/****************/
// 202419017
// Đinh Thế Vinh
/****************/

using Source.Enum;
using Source.Exceptions;

namespace Source.Models.Devices;

public class WirelessMicrophone : TeachingDevice
{
    public int BatteryLevel { get; private set; }
    public int SignalQuality { get; private set; }

    private AlertSeverity? _lastBatteryAlert;
    private AlertSeverity? _lastSignalAlert;

    public WirelessMicrophone(string id, string name, int batteryLevel = 100, int signalQuality = 100)
    : base(id, name)
    {
        ValidateReading(batteryLevel);
        ValidateReading(signalQuality);

        BatteryLevel = batteryLevel;
        SignalQuality = signalQuality;
    }

    public void UpdateBatteryLevel(int newLevel)
    {
        ValidateReading(newLevel);
        BatteryLevel = newLevel;

        AlertSeverity? currentAlert = GetBatteryAlert(newLevel);

        if (currentAlert == _lastBatteryAlert)
            return;

        _lastBatteryAlert = currentAlert;

        if (currentAlert == AlertSeverity.Critical)
            RaiseAlert("Wireless microphone battery is empty.", AlertSeverity.Critical);
        else if (currentAlert == AlertSeverity.Warning)
            RaiseAlert("Wireless microphone battery is low.", AlertSeverity.Warning);
    }

    public void UpdateSignalQuality(int newQuality)
    {
        ValidateReading(newQuality);
        SignalQuality = newQuality;

        AlertSeverity? currentAlert = GetSignalAlert(newQuality);

        if (currentAlert == _lastSignalAlert)
            return;

        _lastSignalAlert = currentAlert;

        if (currentAlert == AlertSeverity.Critical)
            RaiseAlert("Wireless microphone signal is lost.", AlertSeverity.Critical);
        else if (currentAlert == AlertSeverity.Warning)
            RaiseAlert("Wireless microphone signal is weak.", AlertSeverity.Warning);
    }

    private void ValidateReading(int value)
    {
        if (value < 0 || value > 100)
            throw new InvalidDeviceReadingException(Id, value);
    }

    private static AlertSeverity? GetBatteryAlert(int level)
    {
        if (level == 0)
            return AlertSeverity.Critical;

        if (level < 30)
            return AlertSeverity.Warning;

        return null;
    }

    private static AlertSeverity? GetSignalAlert(int quality)
    {
        if (quality == 0)
            return AlertSeverity.Critical;

        if (quality < 40)
            return AlertSeverity.Warning;

        return null;
    }
}
