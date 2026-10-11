using System;
using System.Collections.Generic;

public class LabRoom
{
    private string _roomId;
    private string _roomName;
    private int _capacity;
    private List<Device> _devices;

    public string RoomId
    {
        get => _roomId;
    }

    public string RoomName
    {
        get => _roomName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Room name cannot be null or empty.");
            }

            _roomName = value;
        }
    }

    public int Capacity
    {
        get => _capacity;
    }

    public List<Device> Devices
    {
        get => new List<Device>(_devices);
    }

    public LabRoom(string roomId, string roomName, int capacity)
    {
        if (string.IsNullOrWhiteSpace(roomId))
        {
            throw new ArgumentException("Room ID cannot be null or empty.");
        }

        if (capacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than zero.");
        }

        _roomId = roomId;
        RoomName = roomName;
        _capacity = capacity;
        _devices = new List<Device>();
    }


    public void AddDevice(Device device)
    {
        if (device == null)
        {
            throw new ArgumentNullException(nameof(device));
        }

        if (_devices.Count >= _capacity)
        {
            throw new InvalidOperationException("The room has reached its maximum capacity.");
        }

        foreach (Device existingDevice in _devices)
        {
            if (existingDevice.DeviceId == device.DeviceId)
            {
                throw new InvalidOperationException("A device with the same ID already exists in this room.");
            }
        }

        _devices.Add(device);
    }

    public bool RemoveDevice(string deviceId)
    {
        if (string.IsNullOrWhiteSpace(deviceId))
        {
            throw new ArgumentException("Device ID cannot be null or empty.", nameof(deviceId));
        }

        for (int i = 0; i < _devices.Count; i++)
        {
            if (_devices[i].DeviceId == deviceId)
            {
                _devices.RemoveAt(i);
                return true;
            }
        }

        return false;
    }

    public Device? FindDevice(string deviceId)
    {
        if (string.IsNullOrWhiteSpace(deviceId))
        {
            throw new ArgumentException("Device ID cannot be null or empty.", nameof(deviceId));
        }

        foreach (Device device in _devices)
        {
            if (device.DeviceId == deviceId)
            {
                return device;
            }
        }

        return null;
    }

    public decimal CalculateAnnualMaintenanceCost()
    {
        decimal totalCost = 0;

        foreach (Device device in _devices)
        {
            totalCost += device.CalculateAnnualMaintenanceCost();
        }

        return totalCost;
    }

    public List<Device> GetDevicesRequiringMaintenance()
    {
        List<Device> result = new List<Device>();

        foreach (Device device in _devices)
        {
            bool isUnderMaintenance = device.Status == DeviceStatus.UnderMaintenance;

            bool hasBeenUsedOverFiveYears = DateTime.Now.Year - device.YearOfUse > 5;

            if (isUnderMaintenance || hasBeenUsedOverFiveYears)
            {
                result.Add(device);
            }
        }

        return result;
    }

}