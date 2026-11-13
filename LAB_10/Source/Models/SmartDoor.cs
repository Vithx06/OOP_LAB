/****************/
// 202419017
// Đinh Thế Vinh
/****************/

using System;
using Source.Interfaces;

namespace Source.Models;

public class SmartDoor : IDevice, ILockable
{
    private readonly string _name;

    public SmartDoor(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Device name cannot be empty.", nameof(name));

        _name = name;
    }

    public void TurnOn()
    {
        Console.WriteLine($"{_name}: ON");
    }

    public void TurnOff()
    {
        Console.WriteLine($"{_name}: OFF");
    }

    public void Lock()
    {
        Console.WriteLine($"{_name}: LOCKED");
    }

    public void Unlock()
    {
        Console.WriteLine($"{_name}: UNLOCKED");
    }
}
