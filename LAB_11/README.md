# LAB 11 – Trung tâm giám sát an toàn Campus Safety Hub

## Thông tin sinh viên thực hiện

**Họ và tên: Đinh Thế Vinh**\
**MSSV: 202419017**\
**Mã lớp: 174373**

## 1. Bài toán

Xây dựng chương trình mô phỏng trung tâm giám sát an toàn cho một phòng máy. Hệ thống tích hợp cảm biến khói cũ, phát cảnh báo cho nhiều bên nhận và điều phối các thiết bị ứng phó khẩn cấp. Áp dụng các mẫu thiết kế Adapter, Observer và Mediator để chuyển đổi giao diện cảm biến, quản lý việc nhận cảnh báo và phối hợp các thiết bị mà không tạo phụ thuộc trực tiếp giữa chúng.

Chương trình chạy trên Console; dashboard, email và các thao tác thiết bị đều được mô phỏng. Hệ thống không yêu cầu giao diện đồ họa, cơ sở dữ liệu, kết nối mạng hoặc gửi email thật.

## 2. Các chức năng chính

- Tích hợp `LegacySmokeDetector` qua `SmokeSensorAdapter` thực thi `ISensor`, chuyển thang đo 0–1000 sang phần trăm 0–100 và trả đơn vị `% smoke`.
- Đọc trực tiếp trạng thái mới nhất của cảm biến cũ, không sao chép trạng thái và không sửa logic lớp cũ.
- Phân loại mức khói tại `SensorMonitor`: dưới 50% không cảnh báo, từ 50% đến dưới 70% là `WARNING`, từ 70% trở lên là `CRITICAL`.
- Tạo dữ liệu cảnh báo bất biến `AlertEvent`, gồm tên cảm biến, giá trị, đơn vị, mức độ và thời điểm phát sinh.
- Đăng ký, hủy đăng ký và đăng ký lại observer qua `IAlertObserver`, không cho cùng một observer đăng ký hai lần.
- Thông báo trên bản sao danh sách observer, cho phép thay đổi đăng ký trong khi thông báo; bắt và ghi lỗi từng observer để các bên nhận còn lại tiếp tục hoạt động.
- Hiển thị cảnh báo trên dashboard, mô phỏng gửi email và lưu nhật ký cảnh báo trong bộ nhớ.
- Chuyển riêng cảnh báo `CRITICAL` thành sự kiện `FIRE_DETECTED` qua `EmergencyBridgeObserver` và `IEmergencyMediator`.
- Điều phối bật báo động, mở khóa cửa và dừng thông gió theo thứ tự thông qua `EmergencyMediator`.
- Minh họa luồng hoạt động trong chương trình chính và kiểm thử các chức năng, ngưỡng biên, tình huống lỗi.

## 3. Cấu trúc thư mục dự án

```text
LAB_11/
├── Problem/                           # Đề bài
│   └── Problem.pdf
├── UML/                               # Sơ đồ thiết kế
│   └── UML.png                        # Sơ đồ UML nghiệp vụ
├── Source/                            # Mã nguồn chương trình
│   ├── Interfaces/                    # Các giao diện
│   │   ├── ISensor.cs
│   │   ├── IAlertObserver.cs
│   │   └── IEmergencyMediator.cs
│   ├── Models/                        # Dữ liệu và mô hình thiết bị
│   │   ├── AlertEvent.cs
│   │   ├── Legacy/                    # Cảm biến cũ theo mã đề bài
│   │   │   └── LegacySmokeDetector.cs
│   │   └── Devices/                   # Thiết bị ứng phó khẩn cấp
│   │       ├── Alarm.cs
│   │       ├── DoorController.cs
│   │       └── VentilationController.cs
│   ├── Patterns/                      # Các mẫu thiết kế
│   │   ├── Adapters/
│   │   │   └── SmokeSensorAdapter.cs
│   │   ├── Observers/
│   │   │   ├── SensorMonitor.cs
│   │   │   ├── DashboardObserver.cs
│   │   │   ├── EmailAlertObserver.cs
│   │   │   ├── AuditLogObserver.cs
│   │   │   └── EmergencyBridgeObserver.cs
│   │   └── Mediators/
│   │       └── EmergencyMediator.cs
│   ├── Tests/                         # Kiểm thử các mẫu thiết kế
│   │   └── PatternTests.cs
│   ├── Program.cs                     # Chương trình chính
│   └── source.csproj                  # Tệp cấu hình dự án .NET
└── README.md                          # Tài liệu giới thiệu dự án
```

## 4. Kết quả đạt được

- Áp dụng Adapter để tái sử dụng cảm biến cũ qua giao diện chuẩn, giữ phần giám sát độc lập với API của thiết bị cũ.
- Áp dụng Observer để một cảnh báo được nhiều thành phần tiếp nhận và hỗ trợ thay đổi danh sách bên nhận trong thời gian chạy.
- Sử dụng `sealed record AlertEvent` để các observer cùng nhận dữ liệu bất biến, không thay đổi thông tin cảnh báo của nhau.
- Áp dụng Mediator để tập trung logic ứng phó, các thiết bị báo động, cửa và thông gió không cần tham chiếu trực tiếp đến nhau.
- Tích hợp luồng cảm biến cũ → Adapter → Monitor → Observer → Mediator → thiết bị ứng phó; chỉ mức `CRITICAL` kích hoạt các thao tác khẩn cấp.
- Hoàn thiện chương trình minh họa và bộ kiểm thử `PatternTests.RunAll()`; kết quả chạy thực tế đạt **12/12 nhóm kiểm thử**.

Các nhóm kiểm thử gồm:

- **T1:** Chuyển 735 thành 73,5%, kiểm tra đơn vị, giá trị mới sau cập nhật và giới hạn giá trị của setter cảm biến cũ.
- **T2:** Kiểm tra các giá trị 0%, 49,9%, 50%, 69,9%, 70% và 100%; xác nhận mức độ và dữ liệu cảnh báo.
- **T3:** Chống đăng ký trùng, hủy đăng ký nhiều lần và đăng ký lại observer.
- **T4:** Observer tự hủy đăng ký mà không ảnh hưởng các observer còn lại.
- **T5:** Thêm, gỡ observer trong khi thông báo; danh sách mới được áp dụng từ lượt tiếp theo.
- **T6:** Observer lỗi được ghi nhận, các bên nhận sau đó vẫn nhận đúng dữ liệu và mediator vẫn được kích hoạt.
- **T7:** Dashboard, email mô phỏng và nhật ký nhận đúng dữ liệu; cảnh báo không bị thay đổi.
- **T8:** Bridge chỉ chuyển `CRITICAL` thành `FIRE_DETECTED`, làm việc qua `IEmergencyMediator`.
- **T9:** Mediator thực hiện đúng thứ tự ứng phó và bỏ qua sự kiện chưa hỗ trợ.
- **T10:** Tích hợp ba pattern, đối chiếu toàn bộ đầu ra để phát hiện thông báo hoặc thao tác bị thiếu, thừa, sai thứ tự.
- **T11:** Từ chối phụ thuộc null, tên cảm biến và tên sự kiện không hợp lệ.
- **T12:** Monitor sử dụng được một cảm biến giả khác thực thi `ISensor`, không phụ thuộc vào lớp Adapter cụ thể.

## 5. Vị trí sử dụng các mẫu thiết kế

- **Adapter – `SmokeSensorAdapter`:** chứa tham chiếu đến `LegacySmokeDetector`, thực thi `ISensor` và chuyển đổi giá trị bằng phép chia `10.0`.
- **Observer – `SensorMonitor` và các lớp thực thi `IAlertObserver`:** Monitor quản lý đăng ký, quyết định mức độ và phát cảnh báo. `DashboardObserver` hiển thị dữ liệu, `EmailAlertObserver` mô phỏng gửi email, `AuditLogObserver` lưu và in nhật ký, `EmergencyBridgeObserver` chuyển tiếp cảnh báo khẩn cấp.
- **Mediator – `EmergencyMediator`:** nhận sự kiện qua `IEmergencyMediator`, gọi `Alarm.TurnOn()`, `DoorController.Unlock()` và `VentilationController.Stop()` khi nhận `FIRE_DETECTED`.

`Program.cs` chịu trách nhiệm khởi tạo và ghép các đối tượng. `AlertEvent` chỉ chứa dữ liệu cảnh báo; `LegacySmokeDetector` cung cấp API cũ, còn ba lớp thiết bị chỉ thực hiện thao tác riêng của mình.

## 6. Giải thích thiết kế và trade-off

### 6.1. Adapter nên kế thừa hay chứa LegacySmokeDetector?

Chọn chứa tham chiếu đến `LegacySmokeDetector` vì lớp cũ là `sealed`, không thể kế thừa. Cách này giữ nguyên logic lớp cũ và cho Adapter đọc được mọi thay đổi qua cùng một đối tượng, không cần đồng bộ một bản sao trạng thái. Đổi lại, cần thêm một lớp trung gian và truyền cảm biến cũ qua constructor khi khởi tạo.

### 6.2. SensorMonitor có nên biết EmailAlertObserver cụ thể không?

Monitor chỉ cần biết `IAlertObserver`, còn việc tạo và đăng ký email do client thực hiện. Nhờ đó có thể thêm bên nhận mới mà không sửa logic giám sát; T3–T5 kiểm tra đăng ký động và T6 kiểm tra cô lập lỗi. Đổi lại, client phải quản lý đăng ký, và một observer xử lý chậm vẫn làm chậm lượt thông báo vì các lời gọi hiện chạy đồng bộ.

### 6.3. Alarm có cần tham chiếu đến DoorController không?

Không. `Alarm` chỉ bật báo động, `DoorController` chỉ mở khóa cửa, `VentilationController` chỉ dừng thông gió; `EmergencyMediator` quyết định cách phối hợp cả ba. Cách này giảm phụ thuộc giữa thiết bị và tập trung quy trình ứng phó, nhưng mediator cần được sửa khi bổ sung thiết bị hoặc quy trình mới, nên có thể trở nên phức tạp nếu hệ thống mở rộng nhiều.

### 6.4. Ai quyết định giá trị nào là vượt ngưỡng?

`SensorMonitor` quyết định các ngưỡng 50% và 70% sau khi đọc giá trị qua `ISensor`. Adapter chỉ chuyển giao diện và thang đo, còn observer xử lý cảnh báo đã có mức độ; riêng Bridge chỉ quyết định cảnh báo nào cần chuyển sang mediator. Việc tập trung ngưỡng giúp các bên nhận thống nhất cách hiểu, nhưng các ngưỡng hiện được viết cố định và cần sửa Monitor nếu chính sách thay đổi.

### 6.5. Vì sao thông báo trên bản sao danh sách và bắt lỗi từng observer?

`ToArray()` tạo danh sách ổn định cho lượt thông báo hiện tại, tránh lỗi khi observer thêm hoặc gỡ đăng ký trong lúc nhận cảnh báo. Đổi lại, mỗi lượt cần tạo một mảng mới và observer vừa bị gỡ vẫn có thể nhận cảnh báo của lượt hiện tại nếu đã có trong bản sao. Bắt lỗi từng observer giúp tiếp tục thông báo cho các bên còn lại, nhưng hệ thống hiện chỉ ghi lỗi ra console, chưa có cơ chế thử lại hoặc lưu lỗi lâu dài.
