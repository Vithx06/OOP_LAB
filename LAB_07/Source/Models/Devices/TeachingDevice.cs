/****************/
// 202419017
// Đinh Thế Vinh
/****************/

using Source.Enum;
using Source.Interfaces;
using Source.Exceptions;

namespace Source.Models.Devices;

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
    // Phải có thêm tham số roomId vào để đảm bảo tính đa hình
    public abstract bool IsCompatibleWith(string roomId);
    public abstract bool IsReadyForUse();

    public abstract double GetDefaultPriorityScore(string roomId);

    // Borrow kiểm tra các thiết bị được mượn cho phòng tương ứng là hợp lệ
    public void Borrow(string roomId)
    {
        if (!IsReadyForUse())
        {
            throw new DeviceNotReadyException(Id, Status);
        }

        if (!IsCompatibleWith(roomId))
        {
            throw new InvalidOperationException("Device is not compatible with the specified room.");
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
