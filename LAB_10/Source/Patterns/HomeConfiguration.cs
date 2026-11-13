/****************/
// 202419017
// Đinh Thế Vinh
/****************/

using System;

namespace Source.Patterns;

public sealed class HomeConfiguration
{
    // Chỉ khởi tạo cấu hình khi được truy cập lần đầu, bảo đảm an toàn luồng
    private static readonly Lazy<HomeConfiguration> _lazy = new(() => new HomeConfiguration());

    public static HomeConfiguration Instance => _lazy.Value;

    public string HomeName { get; set; } = "My Home";
    public int NightLightLevel { get; set; } = 30;

    private HomeConfiguration()
    {
    }
}
