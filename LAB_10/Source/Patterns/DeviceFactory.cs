/****************/
// 202419017
// Đinh Thế Vinh
/****************/

using System;
using Source.Interfaces;
using Source.Models;

namespace Source.Patterns;

public static class DeviceFactory
{
    public static IDevice Create(string type, string name)
    {
        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentException("Device type cannot be empty.", nameof(type));

        // Chuẩn hóa loại thiết bị trước khi lựa chọn lớp cần tạo
        return type.Trim().ToLowerInvariant() switch
        {
            "light" => new SmartLight(name),
            "fan" => new SmartFan(name),
            "door" => new SmartDoor(name),
            _ => throw new ArgumentException($"Unsupported device type: {type}", nameof(type))
        };
    }
}
