/****************/
// 202419017
// Đinh Thế Vinh
/****************/

using System;

namespace Source.Exceptions;

public class InvalidDeviceReadingException : Exception
{
    public string DeviceId { get; }
    public int InvalidValue { get; }

    public InvalidDeviceReadingException(string deviceId, int invalidValue)
    : base($"Invalid reading value {invalidValue} for device {deviceId}.")
    {
        DeviceId = deviceId;
        InvalidValue = invalidValue;
    }
}
