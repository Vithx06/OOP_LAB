using System;

public class NetworkPrinter : Printer, INetworkable
{
    private string? _ipAddress;
    private bool _isConnected;

    public string? IpAddress => _ipAddress;

    public bool IsConnected => _isConnected;

    public NetworkPrinter(
        string deviceId,
        string deviceName,
        int yearOfUse,
        decimal purchasePrice,
        DeviceStatus status,
        PrinterTechnology printerTechnology,
        int pagesPrinted,
        bool isColorPrinter)
        : base(
            deviceId,
            deviceName,
            yearOfUse,
            purchasePrice,
            status,
            printerTechnology,
            pagesPrinted,
            isColorPrinter)
    {
        _ipAddress = null;
        _isConnected = false;
    }

    public void Connect(string ipAddress)
    {
        if (_isConnected)
        {
            throw new InvalidOperationException("The printer is already connected.");
        }

        if (string.IsNullOrWhiteSpace(ipAddress))
        {
            throw new ArgumentException("IP address cannot be empty.",nameof(ipAddress));
        }

        _ipAddress = ipAddress;
        _isConnected = true;
    }

    public void Disconnect()
    {
        if (!_isConnected)
        {
            throw new InvalidOperationException("The printer is not connected.");
        }

        _ipAddress = null;
        _isConnected = false;
    }
}