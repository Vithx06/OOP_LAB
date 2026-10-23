/****************/
// 202419017
// Đinh Thế Vinh
/****************/
using Source.Enum;
using System.Collections.Generic;

namespace Source.Models.Devices;

public class AirConditionerRemote : TeachingDevice
{
    public IReadOnlySet<string> CompatibleRooms { get; } // Dùng set vì không muốn có trùng lặp

    public AirConditionerRemote(string id, string name, string storageLocation, IEnumerable<string> compatibleRooms)
    : base(id, name, storageLocation)
    {
        if (compatibleRooms == null)
        {
            throw new ArgumentNullException(nameof(compatibleRooms));
        }

        HashSet<string> rooms = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (string roomId in compatibleRooms)
        {
            if (string.IsNullOrWhiteSpace(roomId))
            {
                throw new ArgumentException("Room ID cannot be null or empty.", nameof(compatibleRooms));
            }

            rooms.Add(roomId);
        }

        CompatibleRooms = rooms;
    }

    // Điều khiển điều hòa chỉ tương thích với một số phòng
    public override bool IsCompatibleWith(string roomId)
    {
        if (string.IsNullOrWhiteSpace(roomId))
        {
            return false;
        }
        // Kiểm tra phòng có tương thích không = Phương thức tìm kiếm trong Set
        return CompatibleRooms.Contains(roomId);
    }

    // Điều kiện sẵn sàng
    public override bool IsReadyForUse()
    {
        return Status == DeviceStatus.Available;
    }

    // Điểm ưu tiên = 100 nếu tương thích và 0 nếu ngược lại
    public override double GetDefaultPriorityScore(string roomId)
    {
        return IsCompatibleWith(roomId) ? 100.0 : 0.0;
    }
}