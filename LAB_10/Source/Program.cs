/****************/
// 202419017
// Đinh Thế Vinh
/****************/

using System;
using Source.Interfaces;
using Source.Patterns;
using Source.Tests;

class Program
{
    static int Main(string[] args)
    {
        try
        {
            // 1. Khởi tạo thiết bị thông qua Factory
            Console.WriteLine("1. Tạo các thiết bị thông qua Factory");

            IDevice light = DeviceFactory.Create("light", "Living room light");
            IDevice fan = DeviceFactory.Create("fan", "Living room fan");
            IDevice door = DeviceFactory.Create("door", "Main door");

            if (door is not ILockable lockableDoor)
                throw new InvalidOperationException("Thiết bị cửa không hỗ trợ khóa.");

            Console.WriteLine("Đã tạo đèn, quạt và cửa thông minh.");

            // 2. Thiết lập và truy cập cấu hình chung
            Console.WriteLine("\n2. Thiết lập cấu hình chung của ngôi nhà");

            HomeConfiguration config1 = HomeConfiguration.Instance;
            HomeConfiguration config2 = HomeConfiguration.Instance;

            config1.HomeName = "Vinh's Home";
            config1.NightLightLevel = 30;

            Console.WriteLine($"Tên ngôi nhà: {config2.HomeName}");
            Console.WriteLine($"Mức sáng ban đêm: {config2.NightLightLevel}%");
            Console.WriteLine($"Hai biến cùng tham chiếu một cấu hình: {ReferenceEquals(config1, config2)}");

            // 3. Điều khiển riêng từng thiết bị qua interface
            Console.WriteLine("\n3. Điều khiển riêng từng thiết bị");

            light.TurnOn();
            fan.TurnOn();
            door.TurnOn();
            lockableDoor.Unlock();

            SmartHomeFacade facade = new SmartHomeFacade(light, fan, lockableDoor);

            // 4. Kích hoạt chế độ đi làm bằng một lời gọi
            Console.WriteLine("\n4. Kích hoạt chế độ đi làm");
            facade.GoToWork();

            // 5. Chế độ đi ngủ sử dụng mức sáng từ cấu hình chung
            Console.WriteLine("\n5. Kích hoạt chế độ đi ngủ");
            facade.GoToSleep();

            // 6. Chạy bộ kiểm thử các mẫu thiết kế
            Console.WriteLine("\n6. Kiểm thử Factory, Singleton và Facade");

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
