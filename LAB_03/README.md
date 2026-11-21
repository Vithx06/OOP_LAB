# LAB 03 – Nhóm dự án và nhân sự

## Thông tin sinh viên thực hiện

**Họ và tên: Đinh Thế Vinh**\
**MSSV: 202419017**\
**Mã lớp: 174373**

## 1. Bài toán

Xây dựng hệ thống quản lý nhóm dự án và nhân sự, bao gồm nhân sự thông thường và kỹ sư phần mềm. Mỗi nhóm có thông tin dự án, trưởng nhóm và danh sách thành viên. Nhân sự tồn tại độc lập với nhóm dự án và có thể tham gia nhiều nhóm khác nhau. Hệ thống áp dụng tính đóng gói, kế thừa, đa hình, nạp chồng constructor và phương thức để quản lý nhân sự, phân công thành viên và tính chi phí hằng tháng.

## 2. Các chức năng chính

- Quản lý mã nhân sự, họ tên và lương cơ bản thông qua lớp `Employee`.
- Quản lý thông tin kỹ sư phần mềm với ngôn ngữ lập trình chính và phụ cấp kỹ thuật thông qua lớp `SoftwareEngineer` kế thừa `Employee`.
- Khởi tạo nhân sự và nhóm dự án bằng các constructor khác nhau.
- Tăng lương theo số tiền cố định hoặc tỷ lệ phần trăm bằng các phương thức nạp chồng `IncreaseSalary()`.
- Tạo nhóm dự án có hoặc chưa có trưởng nhóm; thêm thành viên và tùy chọn đặt thành viên mới làm trưởng nhóm.
- Kiểm tra thành viên theo mã, từ chối thêm trùng mã và không cho phép xóa trưởng nhóm hiện tại khi chưa có người thay thế.
- Thay đổi trưởng nhóm, tự động thêm trưởng nhóm mới vào danh sách nếu chưa là thành viên và giữ trưởng nhóm cũ trong nhóm.
- Tính chi phí hằng tháng của từng nhân sự và tổng chi phí của nhóm; kỹ sư phần mềm có chi phí bằng lương cơ bản cộng phụ cấp kỹ thuật.
- Hiển thị thông tin nhân sự và nhóm dự án thông qua lời gọi đa hình.
- Kiểm tra dữ liệu đầu vào, xử lý các trường hợp không hợp lệ bằng ngoại lệ và minh họa vòng đời đối tượng.

## 3. Cấu trúc thư mục dự án

```text
LAB_03/
├── Problem/                           # Đề bài
│   └── Problem.pdf
├── UML/                               # Sơ đồ thiết kế
│   └── UML.png                        # Sơ đồ UML nghiệp vụ
├── Source/                            # Mã nguồn chương trình
│   ├── Models/                        # Các lớp mô hình và nghiệp vụ
│   │   ├── Employee.cs
│   │   ├── SoftwareEngineer.cs
│   │   └── ProjectTeam.cs
│   ├── Program.cs                     # Chương trình chính và các tình huống kiểm thử
│   └── LAB_03.csproj                  # Tệp cấu hình dự án .NET
└── README.md                          # Tài liệu giới thiệu dự án
```

## 4. Kết quả đạt được

- Vận dụng tính đóng gói để kiểm tra mã, họ tên, lương cơ bản, ngôn ngữ lập trình và phụ cấp khi cập nhật dữ liệu.
- Sử dụng kế thừa để mở rộng thông tin và hành vi của nhân sự thông thường thành kỹ sư phần mềm.
- Áp dụng đa hình thông qua các phương thức `CalculateMonthlyCost()` và `DisplayInfo()`, cho phép nhóm xử lý nhiều loại nhân sự qua kiểu `Employee`.
- Vận dụng nạp chồng constructor và phương thức để hỗ trợ nhiều cách khởi tạo đối tượng, tăng lương và thêm thành viên.
- Thể hiện quan hệ kết tập giữa `ProjectTeam` và `Employee`: các nhóm giữ tham chiếu đến nhân sự, một nhân sự có thể tham gia nhiều nhóm và tiếp tục tồn tại khi một nhóm được thu gom.
- Xây dựng các tình huống kiểm thử trong `Program.cs` cho thao tác quản lý nhóm, tính chi phí, lời gọi đa hình và dữ liệu không hợp lệ.
- Minh họa finalizer trong C# và sử dụng cơ chế thu gom rác để quan sát vòng đời nhóm dự án mà không chủ động hủy các nhân sự được tham chiếu.

## 5. Giải thích thiết kế

### 5.1. Nạp chồng và đa hình được sử dụng ở đâu?

Các constructor, `IncreaseSalary()` và `AddMember()` được nạp chồng để hỗ trợ nhiều cách cung cấp dữ liệu; phiên bản được chọn dựa trên danh sách đối số khi biên dịch. `CalculateMonthlyCost()` và `DisplayInfo()` được ghi đè để thay đổi hành vi theo loại nhân sự khi chạy. Nhờ đó, `ProjectTeam` chỉ cần duyệt danh sách `Employee` mà vẫn tính đúng chi phí và hiển thị đủ thông tin của `SoftwareEngineer`.

### 5.2. Vì sao nhân sự vẫn tồn tại khi nhóm không còn được sử dụng?

Bỏ tham chiếu đến nhóm không có nghĩa là chủ động hủy các nhân sự trong danh sách của nhóm. Trong C#, kết thúc phạm vi không bảo đảm đối tượng được thu gom hoặc finalizer chạy ngay; GC quyết định thời điểm xử lý. Chương trình dùng `GC.Collect()` và `GC.WaitForPendingFinalizers()` để minh họa, còn nhân sự vẫn được sử dụng vì các biến hoặc nhóm khác còn giữ tham chiếu đến họ.
