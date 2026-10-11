using System;

public class Projector : Device
{
    private int _brightness;
    private int _lampHoursUsed;

    public int Brightness
    {
        get => _brightness;
        set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value),"Brightness must be greater than zero."
                );
            }

            _brightness = value;
        }
    }

    public int LampHoursUsed
    {
        get => _lampHoursUsed;
        set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value),"Lamp hours used cannot be negative."
                );
            }

            _lampHoursUsed = value;
        }
    }

    public Projector(
        string deviceId,
        string deviceName,
        int yearOfUse,
        decimal purchasePrice,
        DeviceStatus status,
        int brightness,
        int lampHoursUsed
    ) : base(deviceId, deviceName, yearOfUse, purchasePrice, status)
    {
        Brightness = brightness;
        LampHoursUsed = lampHoursUsed;
    }

    public override decimal CalculateAnnualMaintenanceCost()
    {
        decimal cost = PurchasePrice * 0.03m;

        if (LampHoursUsed > 3000)
        {
            cost += 1500000m;
        }

        return cost;
    }
}