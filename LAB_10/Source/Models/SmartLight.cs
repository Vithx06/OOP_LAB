/****************/
// 202419017
// Đinh Thế Vinh
/****************/

using System;
using Source.Interfaces;

namespace Source.Models;

public class SmartLight : IDevice
{
    private readonly string _name;

    public SmartLight(string name)
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
}
