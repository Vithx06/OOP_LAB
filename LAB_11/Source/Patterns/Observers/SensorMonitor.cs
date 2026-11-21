/****************/
// 202419017
// Đinh Thế Vinh
/****************/

using System;
using System.Collections.Generic;
using Source.Interfaces;
using Source.Models;

namespace Source.Patterns.Observers;

public sealed class SensorMonitor
{
    private readonly List<IAlertObserver> _observers = new();

    public void Attach(IAlertObserver observer)
    {
        ArgumentNullException.ThrowIfNull(observer);

        if (_observers.Contains(observer))
            return;

        _observers.Add(observer);
    }

    public void Detach(IAlertObserver observer)
    {
        ArgumentNullException.ThrowIfNull(observer);

        _observers.Remove(observer);
    }

    public void CheckSensor(string sensorName, ISensor sensor)
    {
        if (string.IsNullOrWhiteSpace(sensorName))
            throw new ArgumentException("Sensor name cannot be empty.", nameof(sensorName));

        ArgumentNullException.ThrowIfNull(sensor);

        double value = sensor.ReadValue();

        // Module giám sát quyết định ngưỡng, các observer chỉ xử lý cảnh báo
        string? severity = value switch
        {
            >= 70 => "CRITICAL",
            >= 50 => "WARNING",
            _ => null
        };

        if (severity is null)
            return;

        AlertEvent alertEvent = new AlertEvent(sensorName, value, sensor.GetUnit(), severity, DateTime.Now);

        Notify(alertEvent);
    }

    private void Notify(AlertEvent alertEvent)
    {
        // Duyệt bản sao, thay đổi đăng ký chỉ có hiệu lực từ lượt thông báo tiếp theo
        foreach (IAlertObserver observer in _observers.ToArray())
        {
            try
            {
                observer.Update(alertEvent);
            }
            catch (Exception ex)
            {
                // Một observer lỗi không ngăn các observer còn lại nhận cảnh báo
                Console.WriteLine(
                    $"[OBSERVER ERROR] {observer.GetType().Name}: " +
                    $"{ex.GetType().Name}: {ex.Message}");
            }
        }
    }
}
