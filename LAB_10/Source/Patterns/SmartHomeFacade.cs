/****************/
// 202419017
// Đinh Thế Vinh
/****************/

using System;
using Source.Interfaces;

namespace Source.Patterns;

public sealed class SmartHomeFacade
{
    private readonly IDevice _light;
    private readonly IDevice _fan;
    private readonly ILockable _door;

    public SmartHomeFacade(IDevice light, IDevice fan, ILockable door)
    {
        _light = light ?? throw new ArgumentNullException(nameof(light));
        _fan = fan ?? throw new ArgumentNullException(nameof(fan));
        _door = door ?? throw new ArgumentNullException(nameof(door));
    }

    // Chế độ đi làm: tắt đèn, tắt quạt và khóa cửa
    public void GoToWork()
    {
        _light.TurnOff();
        _fan.TurnOff();
        _door.Lock();
    }

    // Chế độ đi ngủ: tắt quạt, khóa cửa và bật đèn ngủ
    public void GoToSleep()
    {
        _fan.TurnOff();
        _door.Lock();
        _light.TurnOn();

        int level = HomeConfiguration.Instance.NightLightLevel;
        Console.WriteLine($"Night light level: {level}%");
    }
}
