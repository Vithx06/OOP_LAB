/****************/
// 202419017
// Đinh Thế Vinh
/****************/

using System;
using System.Collections.Generic;
using Source.Enum;
using Source.EventArgs;

namespace Source.Services;

public class MaintenanceService
{
    private readonly List<string> _maintenanceRequests = new();

    public IReadOnlyList<string> MaintenanceRequests => _maintenanceRequests.AsReadOnly();

    public void HandleAlert(object? sender, DeviceAlertEventArgs e)
    {
        if (e.Severity != AlertSeverity.Critical)
            return;

        string request = $"Device {e.DeviceId}: {e.Message}";
        _maintenanceRequests.Add(request);
        Console.WriteLine($"[MAINTENANCE] Created request: {request}");
    }
}
