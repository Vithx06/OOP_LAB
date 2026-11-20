/****************/
// 202419017
// Đinh Thế Vinh
/****************/

using System;
using Source.Interfaces;
using Source.Models.Legacy;

namespace Source.Patterns.Adapters;

public sealed class SmokeSensorAdapter : ISensor
{
    private readonly LegacySmokeDetector _legacy;

    public SmokeSensorAdapter(LegacySmokeDetector legacy)
    {
        _legacy = legacy ?? throw new ArgumentNullException(nameof(legacy));
    }

    public double ReadValue()
    {
        // Đọc trực tiếp cảm biến cũ và chuyển thang 0..1000 thành phần trăm
        return _legacy.GetSmokeLevel() / 10.0;
    }

    public string GetUnit()
    {
        return "% smoke";
    }
}
