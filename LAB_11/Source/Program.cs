/****************/
// 202419017
// Đinh Thế Vinh
/****************/

using System;
using Source.Interfaces;
using Source.Models.Devices;
using Source.Models.Legacy;
using Source.Patterns.Adapters;
using Source.Patterns.Mediators;
using Source.Patterns.Observers;
using Source.Tests;

class Program
{
    static int Main(string[] args)
    {
        try
        {
            Console.WriteLine("CAMPUS SAFETY HUB – GIÁM SÁT AN TOÀN PHÒNG MÁY");

            // 1. Tích hợp cảm biến cũ qua Adapter
            Console.WriteLine("\n1. Khởi tạo cảm biến khói và Adapter");

            string sensorName = "Cảm biến khói phòng máy A";
            LegacySmokeDetector legacy = new LegacySmokeDetector(735);
            ISensor sensor = new SmokeSensorAdapter(legacy);

            Console.WriteLine($"Thiết bị: {legacy.GetDescription()}");
            Console.WriteLine($"Giá trị gốc: {legacy.GetSmokeLevel()}");
            Console.WriteLine($"Sau chuyển đổi: {sensor.ReadValue():F1}{sensor.GetUnit()}");

            // 2. Ghép các thiết bị ứng phó và đăng ký các observer
            Console.WriteLine("\n2. Khởi tạo Mediator và đăng ký các bên nhận cảnh báo");

            Alarm alarm = new Alarm();
            DoorController door = new DoorController();
            VentilationController ventilation = new VentilationController();
            IEmergencyMediator mediator = new EmergencyMediator(alarm, door, ventilation);

            IAlertObserver dashboard = new DashboardObserver();
            IAlertObserver email = new EmailAlertObserver();
            AuditLogObserver audit = new AuditLogObserver();
            IAlertObserver emergencyBridge = new EmergencyBridgeObserver(mediator);
            SensorMonitor monitor = new SensorMonitor();

            monitor.Attach(dashboard);
            monitor.Attach(email);
            monitor.Attach(audit);
            monitor.Attach(emergencyBridge);

            // Đăng ký lại cùng một observer không làm phát sinh thông báo trùng
            monitor.Attach(email);
            Console.WriteLine("Đã đăng ký dashboard, email mô phỏng, nhật ký và cầu nối khẩn cấp.");

            // 3. Dưới ngưỡng cảnh báo, các observer không nhận thông báo
            Console.WriteLine("\n3. Mức khói bình thường: 40% – không phát cảnh báo");
            legacy.SetSmokeLevel(400);
            monitor.CheckSensor(sensorName, sensor);
            Console.WriteLine($"Số bản ghi cảnh báo: {audit.Logs.Count}");

            // 4. WARNING chỉ thông báo, chưa kích hoạt thiết bị ứng phó
            Console.WriteLine("\n4. Mức khói 55% – cảnh báo WARNING");
            legacy.SetSmokeLevel(550);
            monitor.CheckSensor(sensorName, sensor);

            // 5. CRITICAL đi qua Bridge và Mediator để kích hoạt các thiết bị
            Console.WriteLine("\n5. Mức khói 73,5% – cảnh báo CRITICAL và ứng phó khẩn cấp");
            legacy.SetSmokeLevel(735);
            monitor.CheckSensor(sensorName, sensor);

            // 6. Thay đổi bên nhận cảnh báo trong thời gian chạy
            Console.WriteLine("\n6. Hủy đăng ký email, kiểm tra lại ở mức khói 60%");
            monitor.Detach(email);
            legacy.SetSmokeLevel(600);
            monitor.CheckSensor(sensorName, sensor);

            Console.WriteLine("\nĐăng ký lại email và kiểm tra cùng mức khói 60%:");
            monitor.Attach(email);
            monitor.CheckSensor(sensorName, sensor);

            // 7. Hiển thị các cảnh báo đã ghi nhận
            Console.WriteLine("\n7. Nhật ký cảnh báo");

            foreach (string log in audit.Logs)
                Console.WriteLine(log);

            Console.WriteLine($"Tổng số bản ghi: {audit.Logs.Count}");

            // 8. Chạy kiểm thử độc lập với các đối tượng minh họa ở trên
            Console.WriteLine("\n8. Kiểm thử Adapter, Observer và Mediator");
            bool passed = PatternTests.RunAll();
            return passed ? 0 : 1;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[LỖI] {ex.GetType().Name}: {ex.Message}");
            return 1;
        }
    }
}
