/****************/
// 202419017
// Đinh Thế Vinh
/****************/

using System;
using Source.Interfaces;
using Source.Models;

namespace Source.Patterns.Observers;

public sealed class DashboardObserver : IAlertObserver
{
    public void Update(AlertEvent alertEvent)
    {
        ArgumentNullException.ThrowIfNull(alertEvent);

        Console.WriteLine(
            $"[DASHBOARD] {alertEvent.SensorName}: " +
            $"{alertEvent.Value:F1}{alertEvent.Unit} [{alertEvent.Severity}]");
    }
}
