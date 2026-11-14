# LAB 10 – Các mẫu thiết kế Smart Home

## Thông tin sinh viên thực hiện

**Họ và tên: Đinh Thế Vinh**\
**MSSV: 202419017**\
**Mã lớp: 174373**

## 1. Bài toán

Xây dựng ứng dụng Smart Home Mini với ba loại thiết bị: đèn thông minh, quạt thông minh và cửa thông minh. Áp dụng các mẫu thiết kế Factory, Singleton và Facade để tách việc tạo thiết bị khỏi client, chia sẻ cấu hình chung của ngôi nhà và cung cấp các chế độ điều khiển nhiều thiết bị thông qua một thao tác mức cao.

## 2. Các chức năng chính

- Bật và tắt các thiết bị thông qua giao diện `IDevice`; khóa và mở khóa cửa thông qua giao diện `ILockable`.
- Tạo thiết bị theo loại bằng `DeviceFactory`, không phân biệt chữ hoa, chữ thường và bỏ khoảng trắng ở đầu, cuối chuỗi loại thiết bị.
- Kiểm tra tên thiết bị và loại thiết bị hợp lệ; phát sinh ngoại lệ khi dữ liệu không phù hợp.
- Sử dụng một đối tượng `HomeConfiguration` để chia sẻ tên ngôi nhà và mức sáng ban đêm.
- Cung cấp chế độ đi làm: tắt đèn, tắt quạt và khóa cửa.
- Cung cấp chế độ đi ngủ: tắt quạt, khóa cửa, bật đèn và hiển thị mức sáng ban đêm từ cấu hình chung.
- Kiểm thử các tình huống chính của ba mẫu thiết kế và tổng kết số tình huống đạt.

## 3. Cấu trúc thư mục dự án

```text
LAB_10/
├── Problem/                           # Đề bài
│   └── Problem.pdf
├── UML/                               # Sơ đồ thiết kế
│   └── UML.png                        # Sơ đồ UML nghiệp vụ
├── Source/                            # Mã nguồn chương trình
│   ├── Interfaces/                    # Các giao diện
│   │   ├── IDevice.cs
│   │   └── ILockable.cs
│   ├── Models/                        # Các lớp mô hình thiết bị
│   │   ├── SmartLight.cs
│   │   ├── SmartFan.cs
│   │   └── SmartDoor.cs
│   ├── Patterns/                      # Các mẫu thiết kế
│   │   ├── DeviceFactory.cs
│   │   ├── HomeConfiguration.cs
│   │   └── SmartHomeFacade.cs
│   ├── Tests/                         # Kiểm thử các mẫu thiết kế
│   │   └── PatternTests.cs
│   ├── Program.cs                     # Chương trình chính
│   └── Source.csproj                  # Tệp cấu hình dự án .NET
└── README.md                          # Tài liệu giới thiệu dự án
```

## 4. Kết quả đạt được

- Tách biệt hành vi chung của thiết bị và khả năng khóa cửa thông qua hai giao diện riêng.
- Áp dụng Factory để tập trung việc lựa chọn lớp cụ thể và khởi tạo thiết bị, giúp client làm việc qua `IDevice`.
- Áp dụng Singleton với `Lazy<HomeConfiguration>` để khởi tạo cấu hình khi cần và bảo đảm an toàn luồng trong quá trình khởi tạo.
- Áp dụng Facade để tập trung các thao tác của chế độ đi làm và đi ngủ, giảm số lời gọi mà client phải thực hiện.
- Xây dựng bộ kiểm thử `PatternTests.RunAll()` cho năm tình huống T1–T5 trong đề; kết quả chạy kiểm tra đạt 5/5 tình huống.

Các tình huống kiểm thử gồm:

- **T1:** Factory tạo đúng các lớp `SmartLight`, `SmartFan` và `SmartDoor`.
- **T2:** Tạo loại `camera` chưa được hỗ trợ phải phát sinh `ArgumentException`.
- **T3:** Hai lần truy cập `HomeConfiguration.Instance` trả về cùng một đối tượng.
- **T4:** `GoToWork()` in đúng thứ tự đèn tắt, quạt tắt và cửa khóa.
- **T5:** `GoToSleep()` in đúng thứ tự quạt tắt, cửa khóa, đèn bật và mức sáng cấu hình 45%; khôi phục cấu hình sau kiểm thử.

Hiện tại, `Program.cs` vẫn là khung ban đầu và chưa gọi bộ kiểm thử. Kết quả trên được kiểm tra bằng chương trình chạy tạm gọi `PatternTests.RunAll()`.

## 5. Vị trí sử dụng các mẫu thiết kế và hạn chế

- **Factory – `Source/Patterns/DeviceFactory.cs`:** phương thức `Create(type, name)` lựa chọn lớp thiết bị theo chuỗi loại và trả về `IDevice`. Cách tổ chức này tập trung logic khởi tạo, giảm phụ thuộc của client vào các constructor cụ thể.
- **Singleton – `Source/Patterns/HomeConfiguration.cs`:** thuộc tính `Instance` cung cấp cấu hình chung; constructor `private` ngăn tạo đối tượng từ bên ngoài, lớp `sealed` ngăn kế thừa. Các thành phần sử dụng cùng một cấu hình thay vì giữ các bản sao riêng.
- **Facade – `Source/Patterns/SmartHomeFacade.cs`:** hai phương thức `GoToWork()` và `GoToSleep()` phối hợp các thiết bị qua `IDevice` và `ILockable`. Client có thể kích hoạt cả chế độ bằng một lời gọi và vẫn điều khiển riêng từng thiết bị khi cần.

**Hạn chế:** Facade truy cập trực tiếp Singleton nên phụ thuộc vào trạng thái dùng chung, gây khó khăn khi thay thế cấu hình và cô lập các bài kiểm thử. Có thể cải tiến bằng cách truyền cấu hình qua constructor và quản lý vòng đời bằng Dependency Injection. `Lazy<T>` chỉ bảo đảm an toàn luồng khi khởi tạo đối tượng; việc thay đổi các thuộc tính cấu hình vẫn cần được đồng bộ nếu có nhiều luồng cùng ghi.

Mô hình hiện mô phỏng thao tác thiết bị qua console. Mức sáng ban đêm được đọc và thông báo theo yêu cầu đề bài, chưa có chức năng điều chỉnh độ sáng trên thiết bị thực tế.

## 6. Trả lời câu hỏi

### 6.1. Factory đang che giấu quyết định thiết kế nào?

Factory che giấu việc ánh xạ loại thiết bị sang lớp cụ thể và cách gọi constructor tương ứng. Client cung cấp loại, tên thiết bị và nhận về `IDevice`, không cần trực tiếp khởi tạo từng lớp.

### 6.2. Singleton bảo đảm điều gì, nhưng tạo ra rủi ro nào?

Trong thiết kế này, Singleton bảo đảm các lần truy cập `HomeConfiguration.Instance` sử dụng cùng một đối tượng cấu hình. Rủi ro là trạng thái dùng chung có thể bị thay đổi từ nhiều nơi, làm các bài kiểm thử ảnh hưởng lẫn nhau và khó thay thế cấu hình bằng đối tượng giả khi kiểm thử.

### 6.3. Facade khác Factory ở mục đích chính như thế nào?

Factory tập trung vào việc tạo đối tượng. Facade tập trung vào việc cung cấp giao diện đơn giản để phối hợp các đối tượng đã có, chẳng hạn thực hiện nhiều thao tác thiết bị trong một chế độ.

### 6.4. Nếu bỏ SmartHomeFacade, client sẽ phải biết thêm những chi tiết nào?

Client phải biết thiết bị nào cần bật, tắt hoặc khóa, thứ tự gọi từng thao tác trong mỗi chế độ và cách lấy mức sáng từ cấu hình chung. Logic này sẽ phải được viết lại ở các nơi sử dụng nếu không có thành phần điều phối chung.

### 6.5. Khi thêm SmartCamera, những thành phần nào cần sửa?

Cần bổ sung lớp `SmartCamera` triển khai `IDevice`, thêm nhánh `camera` trong `DeviceFactory.Create()` và cập nhật kiểm thử, bao gồm tình huống T2 vốn coi camera là loại chưa hỗ trợ. Client làm việc qua `IDevice` không cần đổi cách gọi bật, tắt. Chỉ cần sửa Facade nếu camera tham gia các chế độ điều khiển.

### 6.6. Constructor của Singleton có mức truy cập nào và vì sao lớp được đánh dấu sealed?

Constructor có mức truy cập `private` để bên ngoài không thể dùng `new HomeConfiguration()`. Lớp được đánh dấu `sealed` để ngăn kế thừa và giữ thiết kế cấu hình không bị mở rộng qua lớp dẫn xuất.

### 6.7. Nếu dùng AddSingleton trong .NET Dependency Injection, có cần tự cài Singleton không?

Không nhất thiết. Nếu mọi thành phần đều lấy cấu hình qua cùng DI container, `AddSingleton` có thể quản lý một đối tượng dùng chung trong vòng đời của service provider. Cách này giúp truyền phụ thuộc rõ ràng và dễ thay thế khi kiểm thử; cần lưu ý các service provider riêng có thể giữ các đối tượng khác nhau.
