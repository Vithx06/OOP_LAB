/****************/
// 202419017
// Đinh Thế Vinh
/****************/

using System;
using System.Collections.Generic;
using Source.EventArgs;

namespace Source.Services;

public class DeviceAuditLogger
{
    private readonly List<string> _logs = new();

    public IReadOnlyList<string> Logs => _logs.AsReadOnly();

    public void HandleStatusChanged(object? sender, DeviceStatusChangedEventArgs e)
    {
        string log = $"[{e.ChangedAt:dd/MM/yyyy HH:mm:ss}] " + $"Device {e.DeviceId}: {e.OldStatus} -> {e.NewStatus}";
        _logs.Add(log);
        Console.WriteLine($"[AUDIT] {log}");
    }
}
