/****************/
// 202419017
// Đinh Thế Vinh
/****************/

using System;
using Source.Interfaces;
using Source.Models;

namespace Source.Patterns.Observers;

public sealed class EmailAlertObserver : IAlertObserver
{
    public void Update(AlertEvent alertEvent)
    {
        ArgumentNullException.ThrowIfNull(alertEvent);

        // Mô phỏng gửi email bằng console, không gửi email thật
        Console.WriteLine(
            $"[EMAIL SIMULATION] [{alertEvent.OccurredAt:dd/MM/yyyy HH:mm:ss}] " +
            $"{alertEvent.SensorName}: {alertEvent.Value:F1}{alertEvent.Unit} " +
            $"[{alertEvent.Severity}]");
    }
}
