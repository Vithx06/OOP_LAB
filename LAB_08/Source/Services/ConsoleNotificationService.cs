/****************/
// 202419017
// Đinh Thế Vinh
/****************/

using System;
using Source.EventArgs;

namespace Source.Services;

public class ConsoleNotificationService
{
    public void HandleAlert(object? sender, DeviceAlertEventArgs e)
    {
        Console.WriteLine($"[{e.OccurredAt:dd/MM/yyyy HH:mm:ss}] [{e.Severity}] " + $"Device {e.DeviceId}: {e.Message}");
    }
}
