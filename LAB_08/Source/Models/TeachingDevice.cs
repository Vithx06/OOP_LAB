/****************/
// 202419017
// Đinh Thế Vinh
/****************/

using Source.Enum;
using Source.EventArgs;

namespace Source.Models;

public abstract class TeachingDevice
{
    public string Id { get; }
    public string Name { get; }
    public DeviceStatus Status { get; private set; }

    public event EventHandler<DeviceStatusChangedEventArgs>? StatusChanged;
    public event EventHandler<DeviceAlertEventArgs>? AlertRaised;

    protected TeachingDevice(string id, string name)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Device ID cannot be empty.", nameof(id));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Device name cannot be empty.", nameof(name));

        Id = id;
        Name = name;
        Status = DeviceStatus.Available;
    }

    public void ChangeStatus(DeviceStatus newStatus)
    {
        if (newStatus == Status)
            return;

        DeviceStatus oldStatus = Status;
        Status = newStatus;

        OnStatusChanged(new DeviceStatusChangedEventArgs(Id, oldStatus, newStatus, DateTime.Now));
    }

    protected void RaiseAlert(string message, AlertSeverity severity)
    {
        OnAlertRaised(new DeviceAlertEventArgs(Id, message, severity, DateTime.Now));
    }

    protected virtual void OnStatusChanged(DeviceStatusChangedEventArgs e)
    {
        StatusChanged?.Invoke(this, e);
    }

    protected virtual void OnAlertRaised(DeviceAlertEventArgs e)
    {
        AlertRaised?.Invoke(this, e);
    }
}