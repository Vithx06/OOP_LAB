/****************/
// 202419017
// Đinh Thế Vinh
/****************/

using System;
using System.Collections.Generic;
using Source.Models;

namespace Source.Services;

public class DeviceMonitoringCoordinator : IDisposable
{
    private readonly HashSet<TeachingDevice> _devices = new();
    private readonly ConsoleNotificationService _notificationService;
    private readonly DeviceAuditLogger _auditLogger;
    private readonly MaintenanceService _maintenanceService;

    private bool _disposed;

    public DeviceMonitoringCoordinator(
        ConsoleNotificationService notificationService,
        DeviceAuditLogger auditLogger,
        MaintenanceService maintenanceService)
    {
        _notificationService = notificationService
            ?? throw new ArgumentNullException(nameof(notificationService));

        _auditLogger = auditLogger
            ?? throw new ArgumentNullException(nameof(auditLogger));

        _maintenanceService = maintenanceService
            ?? throw new ArgumentNullException(nameof(maintenanceService));
    }

    public void Subscribe(TeachingDevice device)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        ArgumentNullException.ThrowIfNull(device);

        if (!_devices.Add(device))
            return;

        device.AlertRaised += _notificationService.HandleAlert;
        device.AlertRaised += _maintenanceService.HandleAlert;
        device.StatusChanged += _auditLogger.HandleStatusChanged;
    }

    public void Unsubscribe(TeachingDevice device)
    {
        ArgumentNullException.ThrowIfNull(device);

        if (!_devices.Remove(device))
            return;

        device.AlertRaised -= _notificationService.HandleAlert;
        device.AlertRaised -= _maintenanceService.HandleAlert;
        device.StatusChanged -= _auditLogger.HandleStatusChanged;
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        foreach (TeachingDevice device in _devices)
        {
            device.AlertRaised -= _notificationService.HandleAlert;
            device.AlertRaised -= _maintenanceService.HandleAlert;
            device.StatusChanged -= _auditLogger.HandleStatusChanged;
        }

        _devices.Clear();
        _disposed = true;
    }
}
