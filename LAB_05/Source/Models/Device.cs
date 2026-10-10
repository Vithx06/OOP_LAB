using System;
public abstract class Device
{
    private string _deviceId;
    private string _deviceName;
    private int _yearOfUse;
    private decimal _purchasePrice;
    private DeviceStatus _status;

    public string DeviceId
    {
        get => _deviceId;
    }

    public string DeviceName
    {
        get => _deviceName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Device name cannot be null or empty.");
            }

            _deviceName = value;
        }
    }

    public int YearOfUse
    {
        get => _yearOfUse;
        set
        {
            if (value > DateTime.Now.Year)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Year of use cannot be in the future.");
            }

            _yearOfUse = value;
        }
    }

    public decimal PurchasePrice
    {
        get => _purchasePrice;
        set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Purchase price must be greater than zero.");
            }

            _purchasePrice = value;
        }
    }

    public DeviceStatus Status
    {
        get => _status;
        set
        {
            if (!Enum.IsDefined(typeof(DeviceStatus), value))
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Invalid device status.");
            }

            _status = value;
        }
    }

    protected Device(string deviceId, string deviceName, int yearOfUse, decimal purchasePrice, DeviceStatus status)
    {
        if (string.IsNullOrWhiteSpace(deviceId))
        {
            throw new ArgumentException("Device ID cannot be null or empty.");
        }

        _deviceId = deviceId;
        DeviceName = deviceName;
        YearOfUse = yearOfUse;
        PurchasePrice = purchasePrice;
        Status = status;
    }

    public abstract decimal CalculateAnnualMaintenanceCost();

    public override string ToString()
    {
        return $"ID: {DeviceId}, " +
               $"Name: {DeviceName}, " +
               $"Year: {YearOfUse}, " +
               $"Price: {PurchasePrice}, " +
               $"Status: {Status}";
    }

}