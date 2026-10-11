using System;

public class Computer : Device, INetworkable
{
    private int _ram;
    private string _processorType;
    private bool _hasDedicatedGpu;

    private string? _ipAddress;
    private bool _isConnected;

    public int Ram
    {
        get => _ram;
        set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value),"RAM must be greater than zero.");
            }

            _ram = value;
        }
    }

    public string ProcessorType
    {
        get => _processorType;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Processor type cannot be null or empty.");
            }

            _processorType = value;
        }
    }

    public bool HasDedicatedGpu
    {
        get => _hasDedicatedGpu;
        set => _hasDedicatedGpu = value;
    }

    public string IpAddress
    {
        get
        {
            if (!_isConnected || _ipAddress == null)
            {
                throw new InvalidOperationException("The computer is not connected to a network.");
            }

            return _ipAddress;
        }
    }

    public bool IsConnected
    {
        get => _isConnected;
    }

    public Computer(
        string deviceId,
        string deviceName,
        int yearOfUse,
        decimal purchasePrice,
        DeviceStatus status,
        int ram,
        string processorType,
        bool hasDedicatedGpu)
        : base(
            deviceId,
            deviceName,
            yearOfUse,
            purchasePrice,
            status)
    {
        Ram = ram;
        ProcessorType = processorType;
        HasDedicatedGpu = hasDedicatedGpu;

        _ipAddress = null;
        _isConnected = false;
    }

    public void Connect(string ipAddress)
    {
        if (string.IsNullOrWhiteSpace(ipAddress))
        {
            throw new ArgumentException("IP address cannot be null or empty.",nameof(ipAddress));
        }

        if (_isConnected)
        {
            throw new InvalidOperationException("The computer is already connected.");
        }

        _ipAddress = ipAddress;
        _isConnected = true;
    }

    public void Disconnect()
    {
        if (!_isConnected)
        {
            throw new InvalidOperationException("The computer is not connected.");
        }

        _ipAddress = null;
        _isConnected = false;
    }

    public override decimal CalculateAnnualMaintenanceCost()
    {
        decimal cost = PurchasePrice * 0.05m;

        if (HasDedicatedGpu)
        {
            cost += PurchasePrice * 0.02m;
        }

        if (DateTime.Now.Year - YearOfUse > 5)
        {
            cost += PurchasePrice * 0.01m;
        }

        return cost;
    }
}