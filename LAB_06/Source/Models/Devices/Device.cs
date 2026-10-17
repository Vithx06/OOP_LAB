/****************/
// 202419017
// Đinh Thế Vinh
/****************/
using System;

public abstract class Device
{
    private string _deviceId;
    private string _name;
    private DeviceStatus _status;
    private int _maxBorrowDays;

    public string DeviceId
    {
        get => _deviceId;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Device ID cannot be null or empty.", nameof(value));
            }

            _deviceId = value;
        }
    }

    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Device name cannot be null or empty.", nameof(value));
            }

            _name = value;
        }
    }

    public DeviceStatus Status
    {
        get => _status;
        private set
        {
            if (!Enum.IsDefined(typeof(DeviceStatus), value))
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Invalid device status.");
            }

            _status = value;
        }
    }

    public int MaxBorrowDays
    {
        get => _maxBorrowDays;
        private set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Maximum borrow days must be greater than zero.");
            }

            _maxBorrowDays = value;
        }
    }

    protected Device(string deviceId, string name, DeviceStatus status, int maxBorrowDays)
    {
        DeviceId = deviceId;
        Name = name;
        Status = status;
        MaxBorrowDays = maxBorrowDays;
    }


    public override string ToString()
    {
        return $"Device ID: {DeviceId}, " + $"Name: {Name}, " + $"Status: {Status}, " + $"Max borrow days: {MaxBorrowDays}";
    }

    public void MarkAsBorrowed()
    {
        if (Status != DeviceStatus.Available)
        {
            throw new DeviceUnavailableException(DeviceId, Status);
        }

        Status = DeviceStatus.Borrowed;
    }


    public void MarkAsAvailable()
    {
        if (Status != DeviceStatus.Borrowed)
        {
            throw new InvalidOperationException("Only borrowed devices can be marked as available.");
        }

        Status = DeviceStatus.Available;
    }

    // Cần xác định công thức tính phí trễ ???
    public decimal CalculateLateFee(int lateDays)
    {
        if (lateDays < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(lateDays), "Late days cannot be negative.");
        }

        return 0m;
    }
}