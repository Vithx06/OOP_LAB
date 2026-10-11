/****************/
// 202419017
// Đinh Thế Vinh
/****************/

using System;
using System.Collections.Generic;

class Program
{
    // Bắt lỗi, ngoại lệ
    static void TestEdge(string description, Action action)
    {
        try
        {
            action();
            Console.WriteLine($"[KHÔNG lỗi] {description}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"[Bắt được ngoại lệ] {description} -> {ex.GetType().Name}: {ex.Message}");
        }
    }
    static void Main()
    {
        // KHỞI TẠO
        // Tạo các thiết bị
        Computer computer1 = new Computer(
            "PC01", "Máy tính phòng A", 2022, 15000000m,
            DeviceStatus.Active, 16, "Intel Core i5", false);

        Computer computer2 = new Computer(
            "PC02", "Máy tính phòng B", 2019, 25000000m,
            DeviceStatus.UnderMaintenance, 32, "Intel Core i7", true);

        Printer printer1 = new Printer(
            "PR01", "Máy in laser", 2023, 5000000m,
            DeviceStatus.Active, PrinterTechnology.Laser, 50000, false);

        Printer printer2 = new Printer(
            "PR02", "Máy in màu", 2020, 8000000m,
            DeviceStatus.Active, PrinterTechnology.Inkjet, 120000, true);

        Projector projector1 = new Projector(
            "PJ01", "Máy chiếu phòng A", 2018, 12000000m,
            DeviceStatus.Active, 3500, 3500);

        // Tạo các phòng thực hành
        LabRoom room1 = new LabRoom("R01", "Phòng thực hành A", 20);
        LabRoom room2 = new LabRoom("R02", "Phòng thực hành B", 15);

        // 1. Thêm thiết bị vào các phòng
        Console.WriteLine("\n1. Thêm thiết bị vào phòng.");

        // Thêm thiết bị vào phòng A
        room1.AddDevice(computer1);
        room1.AddDevice(printer1);
        room1.AddDevice(projector1);

        // Thêm thiết bị vào phòng B
        room2.AddDevice(computer2);
        room2.AddDevice(printer2);

        // TEST

        // 2. Kiểm tra mã thiết bị trùng
        Console.WriteLine("\n2. Thử thêm một thiết bị bị trùng mã.");

        TestEdge("Thêm thiết bị có mã trùng vào phòng A", () =>
        {
            room1.AddDevice(computer1);
        });

        // 3. Hiển thị danh sách thiết bị của từng phòng
        Console.WriteLine("\n3. In danh sách thiết bị trong từng phòng.");

        // Hiển thị thiết bị trong phòng A
        Console.WriteLine("\nTHIẾT BỊ PHÒNG A");
        foreach (Device device in room1.GetDevices())
        {
            Console.WriteLine(device);
        }

        // Hiển thị thiết bị trong phòng B
        Console.WriteLine("\nTHIẾT BỊ PHÒNG B");
        foreach (Device device in room2.GetDevices())
        {
            Console.WriteLine(device);
        }

        // 4. Tính chi phí bảo trì hằng năm
        Console.WriteLine("\n4. Tính tổng chi phí bảo trì dự kiến của mỗi phòng.");

        Console.WriteLine("\nCHI PHÍ BẢO TRÌ ");

        Console.WriteLine($"Phòng A: {room1.CalculateAnnualMaintenanceCost():N0} VND");

        Console.WriteLine($"Phòng B: {room2.CalculateAnnualMaintenanceCost():N0} VND");

        // 5. Liệt kê thiết bị cần bảo trì
        Console.WriteLine("\n5. Liệt kê thiết bị cần bảo trì.");

        Console.WriteLine("\nTHIẾT BỊ CẦN BẢO TRÌ ");

        Console.WriteLine("\nPhòng A:");
        foreach (Device device in room1.GetDevicesRequiringMaintenance())
        {
            Console.WriteLine(device);
        }

        Console.WriteLine("\nPhòng B:");
        foreach (Device device in room2.GetDevicesRequiringMaintenance())
        {
            Console.WriteLine(device);
        }

        // 6 + 7. Kết nối mạng cho các đối tượng thực thi INetworkable và duyệt kiểm tra kết nối mạng thông qua kiểu INetworkable
        Console.WriteLine("\n6. Kết nối mạng cho các đối tượng thực thi INetworkable.");
        Console.WriteLine("\n7. Duyệt các thiết bị mạng thông qua kiểu INetworkable, không phụ thuộc vào lớp cụ thể.");

        Console.WriteLine("\nKIỂM THỬ KẾT NỐI MẠNG ");

        List<INetworkable> networkDevices = new List<INetworkable>
        {
            computer1,
            computer2
        };

        foreach (INetworkable device in networkDevices)
        {
            Console.WriteLine($"\n--- {device} ---");

            TestEdge("Kết nối thiết bị", () =>
            {
                device.Connect("192.168.1.10");

                Console.WriteLine($"IP: {device.IpAddress}");
                Console.WriteLine($"Đã kết nối: {device.IsConnected}");

                device.Disconnect();

                Console.WriteLine($"Sau khi ngắt: {device.IsConnected}");
                Console.WriteLine($"IP sau khi ngắt: '{device.IpAddress}'");
            });
        }

        // 8. Thêm thiết bị khi phòng đã đạt sức chứa tối đa
        Console.WriteLine("\n8. KIỂM THỬ SỨC CHỨA TỐI ĐA CỦA PHÒNG");

        LabRoom testRoom = new LabRoom("R03", "Phòng kiểm thử sức chứa", 1);

        TestEdge("Thêm thiết bị vào phòng đã đạt sức chứa tối đa", () =>
        {
            // Thêm thiết bị thứ nhất
            testRoom.AddDevice(computer1);

            // Thêm thiết bị thứ hai khi phòng đã đầy
            testRoom.AddDevice(printer1);
        });

        // In danh sách thiết bị sau khi kiểm thử
        Console.WriteLine("\nDanh sách thiết bị trong phòng kiểm thử:");

        foreach (Device device in testRoom.GetDevices())
        {
            Console.WriteLine(device);
        }

        // 9. Thử khởi tạo thiết bị với dữ liệu không hợp lệ
        Console.WriteLine("\n9. KIỂM THỬ DỮ LIỆU KHỞI TẠO KHÔNG HỢP LỆ");

        TestEdge("Khởi tạo máy tính với giá mua âm", () =>
        {
            Computer invalidComputer = new Computer(
                "PC_BAD", "Máy tính có dữ liệu không hợp lệ", 2022, -1000000m,
                DeviceStatus.Active, 16, "Intel Core i15", false);

            Console.WriteLine(invalidComputer);
        });

        // 10. Kiểm thử logic xóa thiết bị khỏi phòng
        Console.WriteLine("\n10. KIỂM THỬ LOGIC XÓA THIẾT BỊ");

        LabRoom removeTestRoom = new LabRoom("R04", "Phòng kiểm thử xóa thiết bị", 3);

        removeTestRoom.AddDevice(computer1);
        removeTestRoom.AddDevice(printer1);
        removeTestRoom.AddDevice(projector1);

        TestEdge("Xóa thiết bị không tồn tại", () =>
        {
            removeTestRoom.RemoveDevice("UNKNOWN");
        });

        // In danh sách thiết bị sau khi kiểm thử
        Console.WriteLine("\nDanh sách thiết bị trong phòng kiểm thử:");

        foreach (Device device in  removeTestRoom.GetDevices())
        {
            Console.WriteLine(device);
        }

    }
}