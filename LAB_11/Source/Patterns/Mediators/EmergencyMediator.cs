/****************/
// 202419017
// Đinh Thế Vinh
/****************/

using System;
using Source.Interfaces;
using Source.Models.Devices;

namespace Source.Patterns.Mediators;

public sealed class EmergencyMediator : IEmergencyMediator
{
    private readonly Alarm _alarm;
    private readonly DoorController _door;
    private readonly VentilationController _ventilation;

    public EmergencyMediator(Alarm alarm, DoorController door, VentilationController ventilation)
    {
        _alarm = alarm ?? throw new ArgumentNullException(nameof(alarm));
        _door = door ?? throw new ArgumentNullException(nameof(door));
        _ventilation = ventilation ?? throw new ArgumentNullException(nameof(ventilation));
    }

    public void Notify(string eventName)
    {
        if (string.IsNullOrWhiteSpace(eventName))
            throw new ArgumentException("Event name cannot be empty.", nameof(eventName));

        if (eventName != "FIRE_DETECTED")
            return;

        // Tập trung logic ứng phó, các thiết bị không gọi trực tiếp lẫn nhau
        _alarm.TurnOn();
        _door.Unlock();
        _ventilation.Stop();
    }
}
