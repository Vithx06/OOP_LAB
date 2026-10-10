
using System;

public class Printer : Device
{
    private PrinterTechnology _printerTechnology;
    private int _pagesPrinted;
    private bool _supportsNetwork;
    private bool _isColorPrinter;

    public PrinterTechnology PrinterTechnology
    {
        get => _printerTechnology;
        set
        {
            if (!Enum.IsDefined(typeof(PrinterTechnology), value))
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Invalid printer technology.");
            }

            _printerTechnology = value;
        }
    }

    public int PagesPrinted
    {
        get => _pagesPrinted;
        set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Pages printed cannot be negative.");
            }

            _pagesPrinted = value;
        }
    }

    public bool SupportsNetwork
    {
        get => _supportsNetwork;
        set => _supportsNetwork = value;
    }

    public bool IsColorPrinter
    {
        get => _isColorPrinter;
        set => _isColorPrinter = value;
    }

    public Printer(
        string deviceId,
        string deviceName,
        int yearOfUse,
        decimal purchasePrice,
        DeviceStatus status,
        PrinterTechnology printerTechnology,
        int pagesPrinted,
        bool supportsNetwork,
        bool isColorPrinter)
        : base(deviceId, deviceName, yearOfUse, purchasePrice, status)
    {
        PrinterTechnology = printerTechnology;
        PagesPrinted = pagesPrinted;
        SupportsNetwork = supportsNetwork;
        IsColorPrinter = isColorPrinter;
    }

    public override decimal CalculateAnnualMaintenanceCost()
    {
        decimal cost = PurchasePrice * 0.04m;

        if (PagesPrinted > 100000)
        {
            cost += 500000m;
        }

        if (IsColorPrinter)
        {
            cost += 300000m;
        }

        return cost;
    }

    public override string ToString()
    {
        return base.ToString() +
               $", Technology: {PrinterTechnology}" +
               $", Pages printed: {PagesPrinted}" +
               $", Supports network: {SupportsNetwork}" +
               $", Color printer: {IsColorPrinter}";
    }
}
