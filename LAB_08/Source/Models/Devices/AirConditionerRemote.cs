/****************/
// 202419017
// Đinh Thế Vinh
/****************/

using Source.Enum;

namespace Source.Models.Devices;

public class AirConditionerRemote : TeachingDevice
{
    public string AssignedRoom { get; }
    public bool IsResponsive { get; private set; } = true;

    public AirConditionerRemote(string id, string name, string assignedRoom) : base(id, name)
    {
        if (string.IsNullOrWhiteSpace(assignedRoom))
            throw new ArgumentException("Assigned room cannot be empty.", nameof(assignedRoom));

        AssignedRoom = assignedRoom;
    }

    public void UpdateResponseState(bool isResponsive)
    {
        if (IsResponsive == isResponsive)
            return;

        IsResponsive = isResponsive;

        if (!isResponsive)
            RaiseAlert("Air conditioner remote is not responding.", AlertSeverity.Critical);
    }
}
