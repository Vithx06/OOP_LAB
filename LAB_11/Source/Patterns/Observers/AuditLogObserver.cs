/****************/
// 202419017
// Đinh Thế Vinh
/****************/

using System;
using System.Collections.Generic;
using Source.Interfaces;
using Source.Models;

namespace Source.Patterns.Observers;

public sealed class AuditLogObserver : IAlertObserver
{
    private readonly List<string> _logs = new();

    public IReadOnlyList<string> Logs => _logs.AsReadOnly();

    public void Update(AlertEvent alertEvent)
    {
        ArgumentNullException.ThrowIfNull(alertEvent);

        string log = $"[{alertEvent.OccurredAt:dd/MM/yyyy HH:mm:ss}] " +
            $"{alertEvent.SensorName}: {alertEvent.Value:F1}{alertEvent.Unit} " +
            $"[{alertEvent.Severity}]";

        _logs.Add(log);
        Console.WriteLine($"[AUDIT] {log}");
    }
}
