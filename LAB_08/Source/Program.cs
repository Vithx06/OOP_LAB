/****************/
// 202419017
// Đinh Thế Vinh
/****************/

using System;
using System.Collections.Generic;
using Source.Enum;
using Source.Delegates;
using Source.Models;
using Source.Services;

class Program
{
    // Bắt lỗi, ngoại lệ
    static void TestEdge(string description, Action action)
    {
        Console.WriteLine($"\n{description}");

        try
        {
            action();
            Console.WriteLine("[KHÔNG phát sinh lỗi]");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Bắt được ngoại lệ] {ex.GetType().Name}: {ex.Message}");
        }

        Console.WriteLine();
    }

    static void Main(string[] args)
    {
        // Khởi tạo các thiết bị để kiểm thử
        ProjectorRemote projector = new ProjectorRemote("PR01", "Điều khiển máy chiếu 1", 100);
        AirConditionerRemote airConditioner = new AirConditionerRemote("AC01", "Điều khiển điều hòa 1", "A203");
        WirelessMicrophone microphone = new WirelessMicrophone("MI01", "Micro không dây 1", 100, 100);

        List<TeachingDevice> devices = new List<TeachingDevice>
        {
            projector,
            airConditioner,
            microphone
        };

        // Khởi tạo các subscriber
        ConsoleNotificationService notificationService = new ConsoleNotificationService();
        DeviceAuditLogger auditLogger = new DeviceAuditLogger();
        MaintenanceService maintenanceService = new MaintenanceService();

        // Khởi tạo lớp điều phối đăng ký
        DeviceMonitoringCoordinator coordinator = new DeviceMonitoringCoordinator(notificationService, auditLogger, maintenanceService);

        // Delegate lọc riêng các cảnh báo Critical
        AlertFilter criticalFilter = alert => alert.Severity == AlertSeverity.Critical;

        FilteredAlertSubscriber criticalOnly = new FilteredAlertSubscriber(
            criticalFilter,
            alert => Console.WriteLine($"[CRITICAL FILTER] {alert.DeviceId}: {alert.Message}")
        );

        // 1. Đăng ký các subscriber
        TestEdge("1. Đăng ký các subscriber", () =>
        {
            foreach (TeachingDevice device in devices)
            {
                coordinator.Subscribe(device);
            }

            projector.AlertRaised += criticalOnly.HandleAlert;
            airConditioner.AlertRaised += criticalOnly.HandleAlert;
            microphone.AlertRaised += criticalOnly.HandleAlert;

            Console.WriteLine("Đăng ký subscriber thành công.");
        });

        // 2. Kiểm tra thay đổi trạng thái thiết bị
        TestEdge("2. Kiểm tra thay đổi trạng thái thiết bị", () =>
        {
            int before = auditLogger.Logs.Count;

            projector.ChangeStatus(DeviceStatus.Borrowed);

            // Trạng thái không đổi -> không phát sinh event
            projector.ChangeStatus(DeviceStatus.Borrowed);

            projector.ChangeStatus(DeviceStatus.Available);

            int added = auditLogger.Logs.Count - before;

            Console.WriteLine($"Số bản ghi trạng thái mới: {added}");

            if (added != 2)
            {
                throw new Exception("ChangeStatus phát sinh event không đúng số lần.");
            }
        });

        // 3. Kiểm tra cảnh báo pin của điều khiển máy chiếu
        TestEdge("3. Kiểm tra cảnh báo pin của điều khiển máy chiếu", () =>
        {
            Console.WriteLine("Pin 100% -> 15%:");
            projector.UpdateBatteryLevel(15);

            Console.WriteLine("\nPin 15% -> 10%:");
            projector.UpdateBatteryLevel(10);

            Console.WriteLine("\nPin 10% -> 0%:");
            projector.UpdateBatteryLevel(0);
        });

        // 4. Kiểm tra trạng thái phản hồi của điều khiển điều hòa
        TestEdge("4. Kiểm tra trạng thái phản hồi của điều khiển điều hòa", () =>
        {
            Console.WriteLine("Chuyển từ phản hồi sang không phản hồi:");
            airConditioner.UpdateResponseState(false);

            Console.WriteLine("\nTiếp tục cập nhật trạng thái không phản hồi:");
            airConditioner.UpdateResponseState(false);
        });

        // 5. Kiểm tra pin của micro không dây
        TestEdge("5. Kiểm tra pin của micro không dây", () =>
        {
            Console.WriteLine("Pin 100% -> 25%:");
            microphone.UpdateBatteryLevel(25);

            Console.WriteLine("\nPin 25% -> 0%:");
            microphone.UpdateBatteryLevel(0);
        });

        // 6. Kiểm tra chất lượng tín hiệu của micro không dây
        TestEdge("6. Kiểm tra chất lượng tín hiệu của micro không dây", () =>
        {
            Console.WriteLine("Tín hiệu 100% -> 35%:");
            microphone.UpdateSignalQuality(35);

            Console.WriteLine("\nTín hiệu 35% -> 0%:");
            microphone.UpdateSignalQuality(0);
        });

        // 7. Kiểm tra delegate lọc cảnh báo Critical
        TestEdge("7. Kiểm tra delegate lọc cảnh báo Critical", () =>
        {
            Console.WriteLine("Cảnh báo Warning:");
            projector.UpdateBatteryLevel(50);
            projector.UpdateBatteryLevel(15);

            Console.WriteLine("\nCảnh báo Critical:");
            projector.UpdateBatteryLevel(0);

            Console.WriteLine("\n[CRITICAL FILTER] chỉ được xuất hiện với cảnh báo Critical.");
        });

        // 8. Kiểm tra giá trị pin không hợp lệ
        TestEdge("8. Kiểm tra giá trị pin không hợp lệ", () =>
        {
            projector.UpdateBatteryLevel(120);
        });

        // 9. Kiểm tra chất lượng tín hiệu không hợp lệ
        TestEdge("9. Kiểm tra chất lượng tín hiệu không hợp lệ", () =>
        {
            microphone.UpdateSignalQuality(-10);
        });

        // 10. Hiển thị lịch sử thay đổi trạng thái
        TestEdge("10. Hiển thị lịch sử thay đổi trạng thái", () =>
        {
            Console.WriteLine($"Số bản ghi: {auditLogger.Logs.Count}");

            foreach (string entry in auditLogger.Logs)
            {
                Console.WriteLine(entry);
            }
        });

        // 11. Hiển thị các yêu cầu bảo trì
        TestEdge("11. Hiển thị các yêu cầu bảo trì", () =>
        {
            Console.WriteLine($"Số yêu cầu bảo trì: {maintenanceService.MaintenanceRequests.Count}");

            foreach (string request in maintenanceService.MaintenanceRequests)
            {
                Console.WriteLine(request);
            }
        });

        // 12. Kiểm tra hủy đăng ký event
        TestEdge("12. Kiểm tra hủy đăng ký event", () =>
        {
            projector.AlertRaised -= criticalOnly.HandleAlert;
            airConditioner.AlertRaised -= criticalOnly.HandleAlert;
            microphone.AlertRaised -= criticalOnly.HandleAlert;

            foreach (TeachingDevice device in devices)
            {
                coordinator.Unsubscribe(device);
            }

            int auditCount = auditLogger.Logs.Count;
            int requestCount = maintenanceService.MaintenanceRequests.Count;

            projector.ChangeStatus(DeviceStatus.Borrowed);

            projector.UpdateBatteryLevel(50);
            projector.UpdateBatteryLevel(0);

            if (auditLogger.Logs.Count != auditCount)
            {
                throw new Exception("DeviceAuditLogger vẫn nhận event sau khi Unsubscribe.");
            }

            if (maintenanceService.MaintenanceRequests.Count != requestCount)
            {
                throw new Exception("MaintenanceService vẫn nhận event sau khi Unsubscribe.");
            }

            Console.WriteLine("Các subscriber không còn nhận event sau khi Unsubscribe.");
        });

        // 13. Kiểm tra Dispose
        TestEdge("13. Kiểm tra Dispose", () =>
        {
            coordinator.Dispose();
            Console.WriteLine("Dispose thực hiện thành công.");
        });
    }
}
