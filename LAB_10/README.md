# LAB 10 – Các mẫu thiết kế Smart Home

## Cấu trúc thư mục

```text
LAB_10/
├── Problem/                           # Đề bài (bổ sung Problem.pdf khi có tài liệu)
├── UML/                               # Sơ đồ thiết kế (bổ sung UML.png khi có sơ đồ)
├── Source/
│   ├── Interfaces/
│   │   ├── IDevice.cs
│   │   └── ILockable.cs
│   ├── Models/
│   │   ├── SmartLight.cs
│   │   ├── SmartFan.cs
│   │   └── SmartDoor.cs
│   ├── Patterns/
│   │   ├── DeviceFactory.cs
│   │   ├── HomeConfiguration.cs
│   │   └── SmartHomeFacade.cs
│   ├── Tests/
│   │   └── PatternTests.cs
│   ├── Program.cs
│   └── Source.csproj
└── README.md
```

Thư mục `Source` là dự án .NET hiện có. Các tệp mã nguồn mới là khung ban đầu để triển khai các mẫu Factory, Singleton và Facade.
