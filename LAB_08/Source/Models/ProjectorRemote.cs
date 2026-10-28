/****************/
// 202419017
// Đinh Thế Vinh
/****************/

using Source.Enum;
using Source.Exceptions;

namespace Source.Models;

public class ProjectorRemote : TeachingDevice
{
    public int BatteryLevel { get; private set; }
    private AlertSeverity? _lastBatteryAlert;

    public ProjectorRemote(string id, string name, int batteryLevel = 100) : base(id, name)
    {
        UpdateBatteryLevel(batteryLevel);
    }

    public void UpdateBatteryLevel(int newLevel)
    {
        if (newLevel < 0 || newLevel > 100)
            throw new InvalidDeviceReadingException(Id, newLevel);

        BatteryLevel = newLevel;

        AlertSeverity? currentAlert = null;

        if (newLevel == 0)
            currentAlert = AlertSeverity.Critical;
        else if (newLevel < 20)
            currentAlert = AlertSeverity.Warning;

        if (currentAlert == _lastBatteryAlert)
            return;

        _lastBatteryAlert = currentAlert;

        if (currentAlert == AlertSeverity.Critical)
            RaiseAlert("Projector remote battery is empty.", AlertSeverity.Critical);
        else if (currentAlert == AlertSeverity.Warning)
            RaiseAlert("Projector remote battery is low.", AlertSeverity.Warning);
    }
}