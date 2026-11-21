/****************/
// 202419017
// Đinh Thế Vinh
/****************/

using System;
using System.Collections.Generic;
using System.IO;
using Source.Interfaces;
using Source.Models;
using Source.Models.Devices;
using Source.Models.Legacy;
using Source.Patterns.Adapters;
using Source.Patterns.Mediators;
using Source.Patterns.Observers;

namespace Source.Tests;

public static class PatternTests
{
    public static bool RunAll()
    {
        int passed = 0;

        if (TestEdge("T1. Adapter chuyển thang đo và đọc trạng thái mới của cảm biến", TestAdapter))
            passed++;

        if (TestEdge("T2. Monitor phân loại đúng các ngưỡng cảnh báo", TestThresholds))
            passed++;

        if (TestEdge("T3. Đăng ký trùng và hủy đăng ký observer", TestSubscriptions))
            passed++;

        if (TestEdge("T4. Observer tự hủy đăng ký trong khi nhận cảnh báo", TestSelfDetach))
            passed++;

        if (TestEdge("T5. Thay đổi danh sách đăng ký trong khi thông báo", TestNotificationSnapshot))
            passed++;

        if (TestEdge("T6. Observer lỗi không ngăn các bên nhận còn lại", TestObserverFailure))
            passed++;

        if (TestEdge("T7. Dashboard, email mô phỏng và nhật ký nhận đúng dữ liệu", TestAlertObservers))
            passed++;

        if (TestEdge("T8. Bridge chỉ chuyển CRITICAL qua abstraction của mediator", TestEmergencyBridge))
            passed++;

        if (TestEdge("T9. Mediator phối hợp đúng thứ tự thiết bị", TestEmergencyMediator))
            passed++;

        if (TestEdge("T10. Tích hợp ba pattern từ cảm biến đến thiết bị ứng phó", TestIntegration))
            passed++;

        if (TestEdge("T11. Các thành phần từ chối dữ liệu và phụ thuộc không hợp lệ", TestInvalidInputs))
            passed++;

        Console.WriteLine($"\nKết quả kiểm thử: {passed}/11 tình huống đạt.");
        return passed == 11;
    }

    // Mỗi tình huống được chạy độc lập và chỉ đạt khi các điều kiện đều đúng
    private static bool TestEdge(string description, Action action)
    {
        Console.WriteLine($"\n{description}");

        try
        {
            action();
            Console.WriteLine("[ĐẠT]");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[KHÔNG ĐẠT] {ex.GetType().Name}: {ex.Message}");
            return false;
        }
    }

    private static void TestAdapter()
    {
        LegacySmokeDetector legacy = new LegacySmokeDetector(735);
        ISensor sensor = new SmokeSensorAdapter(legacy);

        Check(sensor.ReadValue() == 73.5, "Giá trị 735 phải chuyển thành 73,5%.");
        Check(sensor.GetUnit() == "% smoke", "Đơn vị cảm biến không đúng.");

        int[] levels = { 0, 500, 700, 1000 };

        foreach (int level in levels)
        {
            legacy.SetSmokeLevel(level);
            Check(sensor.ReadValue() == level / 10.0, "Adapter không đọc trạng thái mới của cảm biến.");
        }

        legacy.SetSmokeLevel(-1);
        Check(sensor.ReadValue() == 0, "Giá trị dưới 0 chưa được lớp cũ giới hạn.");
        legacy.SetSmokeLevel(1001);
        Check(sensor.ReadValue() == 100, "Giá trị trên 1000 chưa được lớp cũ giới hạn.");
    }

    private static void TestThresholds()
    {
        LegacySmokeDetector legacy = new LegacySmokeDetector(0);
        ISensor sensor = new SmokeSensorAdapter(legacy);
        SensorMonitor monitor = new SensorMonitor();
        List<AlertEvent> alerts = new();
        monitor.Attach(new CallbackObserver(alerts.Add));

        int[] levels = { 0, 499, 500, 699, 700, 1000 };
        string?[] severities = { null, null, "WARNING", "WARNING", "CRITICAL", "CRITICAL" };

        for (int i = 0; i < levels.Length; i++)
        {
            legacy.SetSmokeLevel(levels[i]);
            int before = alerts.Count;
            DateTime startedAt = DateTime.Now;
            monitor.CheckSensor("Phòng máy A", sensor);
            DateTime endedAt = DateTime.Now;

            if (severities[i] is null)
            {
                Check(alerts.Count == before, "Mức khói dưới 50% không được phát cảnh báo.");
                continue;
            }

            Check(alerts.Count == before + 1, "Monitor không phát đúng một cảnh báo.");
            AlertEvent alert = alerts[before];
            Check(alert.Severity == severities[i] && alert.Value == levels[i] / 10.0, "Sai mức độ hoặc giá trị cảnh báo.");
            Check(alert.SensorName == "Phòng máy A" && alert.Unit == "% smoke", "Sai tên hoặc đơn vị cảm biến.");
            Check(alert.OccurredAt >= startedAt && alert.OccurredAt <= endedAt, "Thời điểm cảnh báo không hợp lệ.");
        }
    }

    private static void TestSubscriptions()
    {
        SensorMonitor monitor = new SensorMonitor();
        ISensor sensor = new SmokeSensorAdapter(new LegacySmokeDetector(735));
        int received = 0;
        IAlertObserver observer = new CallbackObserver(_ => received++);

        monitor.Attach(observer);
        monitor.Attach(observer);
        monitor.CheckSensor("Phòng máy A", sensor);
        Check(received == 1, "Cùng một observer bị đăng ký hai lần.");

        monitor.Detach(observer);
        monitor.Detach(observer);
        monitor.CheckSensor("Phòng máy A", sensor);
        Check(received == 1, "Observer vẫn nhận cảnh báo sau khi hủy đăng ký.");

        monitor.Attach(observer);
        monitor.CheckSensor("Phòng máy A", sensor);
        Check(received == 2, "Observer không đăng ký lại được sau khi đã bị gỡ.");
    }

    private static void TestSelfDetach()
    {
        SensorMonitor monitor = new SensorMonitor();
        ISensor sensor = new SmokeSensorAdapter(new LegacySmokeDetector(735));
        int selfCount = 0;
        int otherCount = 0;
        IAlertObserver? self = null;
        self = new CallbackObserver(_ =>
        {
            selfCount++;
            monitor.Detach(self!);
        });

        monitor.Attach(self);
        monitor.Attach(new CallbackObserver(_ => otherCount++));
        monitor.CheckSensor("Phòng máy A", sensor);
        monitor.CheckSensor("Phòng máy A", sensor);

        Check(selfCount == 1 && otherCount == 2, "Tự hủy đăng ký làm sai số lần nhận cảnh báo.");
    }

    private static void TestNotificationSnapshot()
    {
        SensorMonitor monitor = new SensorMonitor();
        ISensor sensor = new SmokeSensorAdapter(new LegacySmokeDetector(735));
        int removedCount = 0;
        int addedCount = 0;
        IAlertObserver removed = new CallbackObserver(_ => removedCount++);
        IAlertObserver added = new CallbackObserver(_ => addedCount++);

        monitor.Attach(new CallbackObserver(_ =>
        {
            monitor.Detach(removed);
            monitor.Attach(added);
        }));
        monitor.Attach(removed);

        monitor.CheckSensor("Phòng máy A", sensor);
        Check(removedCount == 1 && addedCount == 0, "Lượt hiện tại không duyệt đúng bản sao danh sách.");

        monitor.CheckSensor("Phòng máy A", sensor);
        Check(removedCount == 1 && addedCount == 1, "Danh sách mới chưa được áp dụng ở lượt tiếp theo.");
    }

    private static void TestObserverFailure()
    {
        SensorMonitor monitor = new SensorMonitor();
        ISensor sensor = new SmokeSensorAdapter(new LegacySmokeDetector(735));
        AlertEvent? before = null;
        AlertEvent? after = null;
        monitor.Attach(new CallbackObserver(alert => before = alert));
        monitor.Attach(new CallbackObserver(_ => throw new InvalidOperationException("Observer kiểm thử bị lỗi.")));
        monitor.Attach(new CallbackObserver(alert => after = alert));

        string output = CaptureOutput(() => monitor.CheckSensor("Phòng máy A", sensor));

        Check(before is not null && ReferenceEquals(before, after), "Các observer không cùng nhận dữ liệu cảnh báo.");
        Check(after!.Value == 73.5 && after.Severity == "CRITICAL", "Dữ liệu cảnh báo bị sai sau khi observer lỗi.");
        Check(output.Contains("[OBSERVER ERROR]") && output.Contains("Observer kiểm thử bị lỗi."), "Monitor chưa ghi lỗi observer.");
    }

    private static void TestAlertObservers()
    {
        DateTime occurredAt = new DateTime(2026, 11, 21, 10, 30, 0);
        AlertEvent alert = new AlertEvent("Phòng máy A", 73.5, "% smoke", "CRITICAL", occurredAt);
        AlertEvent original = alert with { };
        AuditLogObserver audit = new AuditLogObserver();
        IAlertObserver[] observers = { new DashboardObserver(), new EmailAlertObserver(), audit };

        string output = CaptureOutput(() =>
        {
            foreach (IAlertObserver observer in observers)
                observer.Update(alert);
        });

        string message = $"{alert.SensorName}: {alert.Value:F1}{alert.Unit} [{alert.Severity}]";
        string log = $"[{occurredAt:dd/MM/yyyy HH:mm:ss}] {message}";
        string expected = string.Join(Environment.NewLine, $"[DASHBOARD] {message}", $"[EMAIL SIMULATION] {log}", $"[AUDIT] {log}") + Environment.NewLine;

        Check(output == expected, "Các observer hiển thị sai nội dung cảnh báo.");
        Check(audit.Logs.Count == 1 && audit.Logs[0] == log, "Nhật ký chưa lưu đúng cảnh báo.");
        Check(alert == original, "Dữ liệu cảnh báo đã bị thay đổi.");
    }

    private static void TestEmergencyBridge()
    {
        RecordingMediator mediator = new RecordingMediator();
        IAlertObserver bridge = new EmergencyBridgeObserver(mediator);
        AlertEvent warning = new AlertEvent("Phòng máy A", 55, "% smoke", "WARNING", DateTime.Now);
        bridge.Update(warning);
        Check(mediator.Events.Count == 0, "WARNING không được kích hoạt ứng phó khẩn cấp.");

        bridge.Update(warning with { Value = 70, Severity = "CRITICAL" });
        Check(mediator.Events.Count == 1 && mediator.Events[0] == "FIRE_DETECTED", "Bridge chuyển sai sự kiện khẩn cấp.");
    }

    private static void TestEmergencyMediator()
    {
        IEmergencyMediator mediator = new EmergencyMediator(new Alarm(), new DoorController(), new VentilationController());
        string output = CaptureOutput(() => mediator.Notify("FIRE_DETECTED"));

        Check(output == GetEmergencyOutput(), "Mediator thực hiện sai thao tác hoặc thứ tự ứng phó.");
        Check(CaptureOutput(() => mediator.Notify("OTHER_EVENT")) == "", "Sự kiện chưa hỗ trợ không được kích hoạt thiết bị.");
    }

    private static void TestIntegration()
    {
        LegacySmokeDetector legacy = new LegacySmokeDetector(499);
        ISensor sensor = new SmokeSensorAdapter(legacy);
        SensorMonitor monitor = new SensorMonitor();
        AuditLogObserver audit = new AuditLogObserver();
        IEmergencyMediator mediator = new EmergencyMediator(new Alarm(), new DoorController(), new VentilationController());
        monitor.Attach(new DashboardObserver());
        monitor.Attach(new EmailAlertObserver());
        monitor.Attach(audit);
        monitor.Attach(new EmergencyBridgeObserver(mediator));

        Check(CaptureOutput(() => monitor.CheckSensor("Phòng máy A", sensor)) == "", "Mức bình thường không được phát cảnh báo.");

        legacy.SetSmokeLevel(500);
        string warningOutput = CaptureOutput(() => monitor.CheckSensor("Phòng máy A", sensor));
        Check(warningOutput.Contains("[WARNING]") && !warningOutput.Contains("[ALARM]"), "Luồng WARNING xử lý sai.");
        Check(!warningOutput.Contains("[DOOR]") && !warningOutput.Contains("[VENTILATION]"), "WARNING đã kích hoạt cửa hoặc thông gió.");
        Check(audit.Logs.Count == 1, "WARNING chưa được ghi nhật ký.");

        legacy.SetSmokeLevel(700);
        string criticalOutput = CaptureOutput(() => monitor.CheckSensor("Phòng máy A", sensor));
        Check(criticalOutput.Contains("[DASHBOARD]") && criticalOutput.Contains("[EMAIL SIMULATION]"), "Thiếu bên nhận cảnh báo CRITICAL.");
        Check(criticalOutput.Contains("[CRITICAL]") && criticalOutput.EndsWith(GetEmergencyOutput()), "CRITICAL chưa kích hoạt đúng các thiết bị ứng phó.");
        Check(audit.Logs.Count == 2, "CRITICAL chưa được ghi nhật ký.");
    }

    private static void TestInvalidInputs()
    {
        SensorMonitor monitor = new SensorMonitor();
        ISensor sensor = new SmokeSensorAdapter(new LegacySmokeDetector(735));
        Alarm alarm = new Alarm();
        DoorController door = new DoorController();
        VentilationController ventilation = new VentilationController();
        IEmergencyMediator mediator = new EmergencyMediator(alarm, door, ventilation);

        ExpectArgumentException<ArgumentNullException>(() => new SmokeSensorAdapter(null!), "legacy");
        ExpectArgumentException<ArgumentNullException>(() => new EmergencyMediator(null!, door, ventilation), "alarm");
        ExpectArgumentException<ArgumentNullException>(() => new EmergencyMediator(alarm, null!, ventilation), "door");
        ExpectArgumentException<ArgumentNullException>(() => new EmergencyMediator(alarm, door, null!), "ventilation");
        ExpectArgumentException<ArgumentNullException>(() => new EmergencyBridgeObserver(null!), "mediator");
        ExpectArgumentException<ArgumentNullException>(() => monitor.Attach(null!), "observer");
        ExpectArgumentException<ArgumentNullException>(() => monitor.Detach(null!), "observer");
        ExpectArgumentException<ArgumentNullException>(() => monitor.CheckSensor("Phòng máy A", null!), "sensor");

        string[] invalidNames = { null!, "", " \t\n" };

        foreach (string name in invalidNames)
        {
            ExpectArgumentException<ArgumentException>(() => monitor.CheckSensor(name, sensor), "sensorName");
            ExpectArgumentException<ArgumentException>(() => mediator.Notify(name), "eventName");
        }

        IAlertObserver[] observers = { new DashboardObserver(), new EmailAlertObserver(), new AuditLogObserver(), new EmergencyBridgeObserver(mediator) };

        foreach (IAlertObserver observer in observers)
            ExpectArgumentException<ArgumentNullException>(() => observer.Update(null!), "alertEvent");
    }

    private static string GetEmergencyOutput()
    {
        return string.Join(Environment.NewLine, "[ALARM] ON", "[DOOR] UNLOCKED", "[VENTILATION] STOPPED") + Environment.NewLine;
    }

    private static void Check(bool condition, string message)
    {
        if (!condition)
            throw new Exception(message);
    }

    private static void ExpectArgumentException<T>(Action action, string parameterName) where T : ArgumentException
    {
        try
        {
            action();
        }
        catch (T ex)
        {
            Check(ex.GetType() == typeof(T) && ex.ParamName == parameterName, $"Ngoại lệ không đúng loại hoặc tham số {parameterName}.");
            return;
        }

        throw new Exception($"Không phát sinh {typeof(T).Name} cho tham số {parameterName}.");
    }

    // Luôn khôi phục console kể cả khi thao tác được kiểm thử phát sinh lỗi
    private static string CaptureOutput(Action action)
    {
        TextWriter originalOutput = Console.Out;
        using StringWriter writer = new StringWriter();

        try
        {
            Console.SetOut(writer);
            action();
            return writer.ToString();
        }
        finally
        {
            Console.SetOut(originalOutput);
        }
    }

    // Các đối tượng hỗ trợ chỉ dùng trong kiểm thử, không thêm vào mã nghiệp vụ
    private sealed class CallbackObserver : IAlertObserver
    {
        private readonly Action<AlertEvent> _callback;

        public CallbackObserver(Action<AlertEvent> callback)
        {
            _callback = callback;
        }

        public void Update(AlertEvent alertEvent)
        {
            _callback(alertEvent);
        }
    }

    private sealed class RecordingMediator : IEmergencyMediator
    {
        public List<string> Events { get; } = new();

        public void Notify(string eventName)
        {
            Events.Add(eventName);
        }
    }
}
