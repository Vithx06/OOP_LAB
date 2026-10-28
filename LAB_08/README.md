# LAB 08 – Hệ thống giám sát thiết bị và xử lý sự kiện

## Thông tin sinh viên thực hiện

**Họ và tên: Đinh Thế Vinh**  
**MSSV: 202419017**  
**Mã lớp: 174373**

## 1. Bài toán

Xây dựng hệ thống giám sát các thiết bị phục vụ giảng dạy trong trường học, áp dụng delegate và event để thông báo các thay đổi về trạng thái, mức pin, chất lượng tín hiệu và tình trạng hoạt động của thiết bị. Hệ thống cho phép nhiều thành phần tiếp nhận và xử lý sự kiện độc lập, đồng thời hỗ trợ lọc thông báo và quản lý việc đăng ký, hủy đăng ký các bộ xử lý sự kiện.

## 2. Các chức năng chính

- Quản lý thông tin và trạng thái của các thiết bị giảng dạy, bao gồm điều khiển máy chiếu, điều khiển điều hòa và micro không dây.
- Xây dựng các lớp dữ liệu sự kiện để cung cấp thông tin về thay đổi trạng thái và cảnh báo thiết bị.
- Sử dụng event để công bố các sự kiện khi trạng thái thiết bị thay đổi hoặc phát sinh cảnh báo.
- Sử dụng delegate và callback để xây dựng cơ chế lọc thông báo theo điều kiện.
- Xây dựng các thành phần tiếp nhận sự kiện, bao gồm dịch vụ hiển thị cảnh báo, ghi nhật ký thay đổi và tạo yêu cầu bảo trì khi có cảnh báo nghiêm trọng.
- Xây dựng lớp điều phối việc đăng ký và hủy đăng ký các bộ xử lý sự kiện, tránh đăng ký trùng lặp và hỗ trợ giải phóng tài nguyên thông qua `IDisposable`.
- Xử lý các giá trị đo không hợp lệ bằng ngoại lệ tùy chỉnh và kiểm tra các tình huống phát sinh sự kiện.

## 3. Cấu trúc thư mục dự án

```text
LAB_08/
├── Problem/                           # Đề bài
│   └── Problem.pdf
├── UML/                               # Sơ đồ thiết kế
│   └── UML.png               # Sơ đồ UML nghiệp vụ
├── Source/                            # Mã nguồn chương trình
│   ├── Enum/                          # Các kiểu liệt kê
│   │   ├── DeviceStatus.cs
│   │   └── AlertSeverity.cs
│   ├── EventArgs/                     # Dữ liệu sự kiện
│   │   ├── DeviceStatusChangedEventArgs.cs
│   │   └── DeviceAlertEventArgs.cs
│   ├── Delegates/                     # Các kiểu delegate
│   │   └── AlertFilter.cs
│   ├── Exceptions/                    # Các lớp ngoại lệ tùy chỉnh
│   │   └── InvalidDeviceReadingException.cs
│   ├── Models/                        # Các lớp mô hình thiết bị
│   │   ├── TeachingDevice.cs
│   │   ├── ProjectorRemote.cs
│   │   ├── AirConditionerRemote.cs
│   │   └── WirelessMicrophone.cs
│   ├── Services/                      # Các dịch vụ xử lý sự kiện
│   │   ├── ConsoleNotificationService.cs
│   │   ├── DeviceAuditLogger.cs
│   │   ├── MaintenanceService.cs
│   │   ├── DeviceMonitoringCoordinator.cs
│   │   └── FilteredAlertSubscriber.cs
│   ├── Program.cs                     # Chương trình chính
│   └── Source.csproj                  # Tệp cấu hình dự án .NET
└── README.md                          # Tài liệu giới thiệu dự án
```

## 4. Kết quả đạt được

- Vận dụng delegate và event để xây dựng cơ chế giao tiếp giữa các đối tượng theo hướng sự kiện.
- Áp dụng cơ chế publisher–subscriber, cho phép một sự kiện được nhiều thành phần tiếp nhận và xử lý độc lập.
- Tách biệt trách nhiệm phát hiện, công bố sự kiện của thiết bị với trách nhiệm hiển thị cảnh báo, ghi nhật ký và xử lý bảo trì.
- Sử dụng delegate và callback để xây dựng chính sách lọc thông báo linh hoạt theo mức độ cảnh báo.
- Tổ chức việc đăng ký và hủy đăng ký các bộ xử lý sự kiện tập trung, hạn chế đăng ký trùng lặp và hỗ trợ quản lý vòng đời của các đăng ký.
- Kiểm tra tính hợp lệ của mức pin và chất lượng tín hiệu, xử lý các giá trị không hợp lệ bằng ngoại lệ tùy chỉnh.