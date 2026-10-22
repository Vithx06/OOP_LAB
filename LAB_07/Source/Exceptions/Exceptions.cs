/****************/
// 202419017
// Đinh Thế Vinh
/****************/
using Source.Enum;

namespace Source.Exceptions;

// Ngoại lệ trùng ID thiết bị
public class DuplicateIdException : Exception
{
    public DuplicateIdException(string id)
    : base($"A device with ID '{id}' already exists.")
    {
    }
}

// Ngoại lệ không có thiết bị phù hợp
public class NoSuitableItemException : Exception
{
    public NoSuitableItemException()
    : base("No suitable device is available.")
    {
    }
}

// Ngoại lệ thiết bị không có sẵn để mượn
public class DeviceNotReadyException : Exception
{
    public string DeviceId { get; }
    public DeviceStatus CurrentStatus { get; }

    public DeviceNotReadyException(string deviceId, DeviceStatus currentStatus)
    : base($"Device '{deviceId}' is not ready for use. Current status: {currentStatus}.")
    {
        DeviceId = deviceId;
        CurrentStatus = currentStatus;
    }
}
