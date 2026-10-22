/****************/
// 202419017
// Đinh Thế Vinh
/****************/

using Source.Enum;
using Source.Interfaces;
using Source.Exceptions;

namespace Source.Models;

public abstract class TeachingDevice : IIdentifiable
{
    private string _id;
    private string _name;
    private string _storageLocation;
    private DeviceStatus _status;

    public string Id
    {
        get => _id;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Device ID cannot be null or empty.", nameof(value));
            }

            _id = value;
        }
    }

    public string Name
    {
        get => _name;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Device name cannot be null or empty.", nameof(value));
            }

            _name = value;
        }
    }

    public string StorageLocation
    {
        get => _storageLocation;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Storage location cannot be null or empty.", nameof(value));
            }

            _storageLocation = value;
        }
    }

    public DeviceStatus Status
    {
        get => _status;
        protected set
        {
            if (!System.Enum.IsDefined(typeof(DeviceStatus), value))
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Invalid device status.");
            }

            _status = value;
        }
    }

    protected TeachingDevice(string id, string name, string storageLocation)
    {
        Id = id;
        Name = name;
        StorageLocation = storageLocation;
        Status = DeviceStatus.Available;
    }

    public abstract bool IsReadyForUse();

    public abstract double GetDefaultPriorityScore();

    public void Borrow()
    {
        if (!IsReadyForUse())
        {
            throw new DeviceNotReadyException(Id, Status);
        }

        Status = DeviceStatus.Borrowed;
    }

    public void Return()
    {
        if (Status != DeviceStatus.Borrowed)
        {
            throw new InvalidOperationException("Device is not currently borrowed.");
        }

        Status = DeviceStatus.Available;
    }
}
