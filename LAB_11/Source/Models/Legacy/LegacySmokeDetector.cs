/****************/
// 202419017
// Đinh Thế Vinh
/****************/

using System;

namespace Source.Models.Legacy;

public sealed class LegacySmokeDetector
{
    private int smokeLevel;

    public LegacySmokeDetector(int smokeLevel)
    {
        this.smokeLevel = smokeLevel;
    }

    public int GetSmokeLevel() => smokeLevel; // 0..1000

    public string GetDescription() => "Legacy smoke detector";

    public void SetSmokeLevel(int value)
    {
        smokeLevel = Math.Clamp(value, 0, 1000);
    }
}
