/****************/
// 202419017
// Đinh Thế Vinh
/****************/

using System;
using System.IO;
using Source.Interfaces;
using Source.Models;
using Source.Patterns;

namespace Source.Tests;

public static class PatternTests
{
    public static bool RunAll()
    {
        int passed = 0;

        if (TestEdge("T1. Factory tạo đúng loại thiết bị", TestFactoryTypes))
            passed++;

        if (TestEdge("T2. Factory từ chối loại camera chưa hỗ trợ", TestUnsupportedDevice))
            passed++;

        if (TestEdge("T3. Singleton trả về cùng một đối tượng", TestSingleton))
            passed++;

        if (TestEdge("T4. Kiểm tra chế độ đi làm", TestGoToWork))
            passed++;

        if (TestEdge("T5. Kiểm tra chế độ đi ngủ", TestGoToSleep))
            passed++;

        Console.WriteLine($"\nKết quả kiểm thử: {passed}/5 tình huống đạt.");
        return passed == 5;
    }

    // Mỗi tình huống được chạy độc lập để lỗi không làm dừng các kiểm tra còn lại
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

    private static void TestFactoryTypes()
    {
        IDevice light = DeviceFactory.Create("light", "Living room light");
        IDevice fan = DeviceFactory.Create("fan", "Living room fan");
        IDevice door = DeviceFactory.Create("door", "Main door");

        if (light is not SmartLight || fan is not SmartFan || door is not SmartDoor)
            throw new Exception("Factory không trả về đúng loại thiết bị.");

        Console.WriteLine("Factory tạo đúng SmartLight, SmartFan và SmartDoor.");
    }

    private static void TestUnsupportedDevice()
    {
        try
        {
            DeviceFactory.Create("camera", "Main camera");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Bắt được ngoại lệ mong đợi: {ex.Message}");
            return;
        }

        throw new Exception("Factory không phát sinh ArgumentException cho loại camera.");
    }

    private static void TestSingleton()
    {
        HomeConfiguration config1 = HomeConfiguration.Instance;
        HomeConfiguration config2 = HomeConfiguration.Instance;

        if (!ReferenceEquals(config1, config2))
            throw new Exception("Hai biến cấu hình không cùng tham chiếu một đối tượng.");

        Console.WriteLine("Hai lần truy cập Instance trả về cùng một đối tượng.");
    }

    private static SmartHomeFacade CreateFacade()
    {
        IDevice light = DeviceFactory.Create("light", "Living room light");
        IDevice fan = DeviceFactory.Create("fan", "Living room fan");
        IDevice door = DeviceFactory.Create("door", "Main door");

        if (door is not ILockable lockableDoor)
            throw new Exception("Thiết bị cửa không hỗ trợ khóa.");

        return new SmartHomeFacade(light, fan, lockableDoor);
    }

    private static void TestGoToWork()
    {
        SmartHomeFacade facade = CreateFacade();
        string output = CaptureOutput(facade.GoToWork);
        string expected = string.Join(Environment.NewLine,
            "Living room light: OFF",
            "Living room fan: OFF",
            "Main door: LOCKED") + Environment.NewLine;

        Console.Write(output);

        if (output != expected)
            throw new Exception("Chế độ đi làm không thực hiện đúng các thao tác hoặc thứ tự.");
    }

    private static void TestGoToSleep()
    {
        SmartHomeFacade facade = CreateFacade();
        HomeConfiguration config = HomeConfiguration.Instance;
        int originalLevel = config.NightLightLevel;

        try
        {
            config.NightLightLevel = 45;

            string output = CaptureOutput(facade.GoToSleep);
            string expected = string.Join(Environment.NewLine,
                "Living room fan: OFF",
                "Main door: LOCKED",
                "Living room light: ON",
                "Night light level: 45%") + Environment.NewLine;

            Console.Write(output);

            if (output != expected)
                throw new Exception("Chế độ đi ngủ không thực hiện đúng thao tác hoặc mức sáng cấu hình.");
        }
        finally
        {
            // Khôi phục cấu hình chung để không ảnh hưởng các lần chạy sau
            config.NightLightLevel = originalLevel;
        }
    }

    // Thu kết quả console để kiểm tra, luôn khôi phục đầu ra kể cả khi có lỗi
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
}
