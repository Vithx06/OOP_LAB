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

        if (TestEdge("T6. Factory chuẩn hóa chuỗi loại thiết bị", TestNormalizedDeviceTypes))
            passed++;

        if (TestEdge("T7. Factory từ chối loại null, rỗng hoặc chỉ có khoảng trắng", TestInvalidDeviceTypes))
            passed++;

        if (TestEdge("T8. Các thiết bị từ chối tên không hợp lệ", TestInvalidDeviceNames))
            passed++;

        if (TestEdge("T9. Facade từ chối các phụ thuộc null", TestNullFacadeDependencies))
            passed++;

        if (TestEdge("T10. Chế độ ngủ đọc mức sáng mới nhất từ cấu hình chung", TestUpdatedNightLightLevel))
            passed++;

        Console.WriteLine($"\nKết quả kiểm thử: {passed}/10 tình huống đạt.");
        return passed == 10;
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

    private static void TestNormalizedDeviceTypes()
    {
        string[] types = { "light", "fan", "door" };
        Type[] expectedTypes = { typeof(SmartLight), typeof(SmartFan), typeof(SmartDoor) };

        for (int i = 0; i < types.Length; i++)
        {
            string[] inputs = { types[i].ToUpperInvariant(), $"  {types[i]}  ", $"\t{types[i].ToUpperInvariant()}\n" };

            foreach (string input in inputs)
            {
                IDevice device = DeviceFactory.Create(input, "Test device");

                if (device.GetType() != expectedTypes[i])
                    throw new Exception($"Factory chuẩn hóa sai loại {types[i]}.");
            }
        }
    }

    private static void TestInvalidDeviceTypes()
    {
        string[] invalidTypes = { null!, "", " \t\n" };

        foreach (string type in invalidTypes)
        {
            ExpectArgumentException<ArgumentException>(
                () => DeviceFactory.Create(type, "Test device"), "type");
        }
    }

    private static void TestInvalidDeviceNames()
    {
        string[] types = { "light", "fan", "door" };
        string[] invalidNames = { null!, "", " \t\n" };

        foreach (string type in types)
        {
            foreach (string name in invalidNames)
            {
                ExpectArgumentException<ArgumentException>(
                    () => DeviceFactory.Create(type, name), "name");
            }
        }
    }

    private static void TestNullFacadeDependencies()
    {
        IDevice light = DeviceFactory.Create("light", "Test light");
        IDevice fan = DeviceFactory.Create("fan", "Test fan");
        ILockable door = (ILockable)DeviceFactory.Create("door", "Test door");

        ExpectArgumentException<ArgumentNullException>(
            () => new SmartHomeFacade(null!, fan, door), "light");
        ExpectArgumentException<ArgumentNullException>(
            () => new SmartHomeFacade(light, null!, door), "fan");
        ExpectArgumentException<ArgumentNullException>(
            () => new SmartHomeFacade(light, fan, null!), "door");
    }

    private static void TestUpdatedNightLightLevel()
    {
        SmartHomeFacade facade = CreateFacade();
        HomeConfiguration config1 = HomeConfiguration.Instance;
        HomeConfiguration config2 = HomeConfiguration.Instance;
        int originalLevel = config1.NightLightLevel;

        try
        {
            // Dùng cùng một facade sau mỗi lần cập nhật, gồm cả hai mức biên 0% và 100%
            int[] levels = { 0, 100, 65 };

            foreach (int level in levels)
            {
                config1.NightLightLevel = level;

                if (config2.NightLightLevel != level)
                    throw new Exception("Thay đổi cấu hình không được chia sẻ giữa hai biến.");

                string output = CaptureOutput(facade.GoToSleep);
                string expected = string.Join(Environment.NewLine,
                    "Living room fan: OFF",
                    "Main door: LOCKED",
                    "Living room light: ON",
                    $"Night light level: {level}%") + Environment.NewLine;

                Console.Write(output);

                if (output != expected)
                    throw new Exception($"Chế độ ngủ không áp dụng mức sáng mới: {level}%.");
            }
        }
        finally
        {
            config1.NightLightLevel = originalLevel;
        }
    }

    // Chỉ chấp nhận đúng loại ngoại lệ và tham số mong đợi
    private static void ExpectArgumentException<T>(Action action, string parameterName)
        where T : ArgumentException
    {
        try
        {
            action();
        }
        catch (T ex)
        {
            if (ex.GetType() != typeof(T) || ex.ParamName != parameterName)
                throw new Exception($"Ngoại lệ không đúng loại hoặc tham số {parameterName}.", ex);

            Console.WriteLine($"Bắt được {ex.GetType().Name} cho tham số {parameterName}.");
            return;
        }

        throw new Exception($"Không phát sinh {typeof(T).Name} cho tham số {parameterName}.");
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
