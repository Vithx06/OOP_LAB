# LAB 04 – Hệ thống tính lương và thưởng nhân sự

## Thông tin sinh viên thực hiện

**Họ và tên: Đinh Thế Vinh**\
**MSSV: 202419017**\
**Mã lớp: 174373**

## 1. Bài toán

Xây dựng hệ thống tính thu nhập hằng tháng cho ba loại nhân sự: nhân viên hưởng lương cố định, nhân viên hưởng lương theo giờ và nhân viên kinh doanh hưởng lương cơ bản kèm hoa hồng. Áp dụng tính đóng gói, kế thừa, đa hình, nạp chồng constructor và phương thức để quản lý thông tin nhân sự, ghi nhận thưởng và tổng hợp bảng lương theo từng kỳ.

## 2. Các chức năng chính

- Quản lý mã nhân sự, họ tên, phòng ban và tổng thưởng trong tháng thông qua lớp `Employee`.
- Tính thu nhập của nhân viên lương cố định bằng lương tháng, phụ cấp trách nhiệm và thưởng.
- Tính thu nhập của nhân viên theo giờ, áp dụng hệ số 1,5 cho số giờ vượt ngưỡng 160 giờ và cộng thưởng trong tháng.
- Tính thu nhập của nhân viên kinh doanh bằng lương cơ bản, hoa hồng theo doanh số và thưởng; hỗ trợ cập nhật doanh số.
- Ghi nhận thưởng bằng ba phiên bản nạp chồng `AddBonus()`: thưởng cố định, thưởng cố định kèm lý do và thưởng theo tỷ lệ của giá trị tham chiếu kèm lý do.
- Quản lý danh sách nhân sự theo kỳ lương, từ chối thêm nhân sự trùng mã và tìm nhân sự theo mã.
- Tính tổng bảng lương, tổng thu nhập theo phòng ban và tìm nhân sự có thu nhập cao nhất.
- Hiển thị bảng lương bằng lời gọi đa hình qua kiểu chung `Employee`.
- Kiểm tra dữ liệu đầu vào, xử lý ngoại lệ và các trường hợp danh sách rỗng hoặc không tìm thấy nhân sự.

## 3. Cấu trúc thư mục dự án

```text
LAB_04/
├── Problem/                           # Đề bài
│   └── Problem.pdf
├── UML/                               # Sơ đồ thiết kế
│   └── UML.png                        # Sơ đồ UML nghiệp vụ
├── Source/                            # Mã nguồn chương trình
│   ├── Models/                        # Các lớp mô hình và nghiệp vụ
│   │   ├── Employee.cs
│   │   ├── SalariedEmployee.cs
│   │   ├── HourlyEmployee.cs
│   │   ├── SalesEmployee.cs
│   │   └── Payroll.cs
│   ├── Program.cs                     # Chương trình chính và các tình huống kiểm thử
│   └── Soucre.csproj                  # Tệp cấu hình dự án .NET
└── README.md                          # Tài liệu giới thiệu dự án
```

## 4. Kết quả đạt được

- Vận dụng tính đóng gói để kiểm tra thông tin nhân sự, khoản thưởng và các thành phần thu nhập khi cập nhật dữ liệu.
- Sử dụng kế thừa để xây dựng ba loại nhân sự từ lớp `Employee`, mỗi loại có thông tin và công thức tính thu nhập riêng.
- Áp dụng ghi đè các phương thức `CalculateGrossPay()`, `GetEmployeeType()` và `DisplayPayrollInfo()` để thể hiện hành vi của từng loại nhân sự.
- Vận dụng nạp chồng constructor và phương thức để hỗ trợ nhiều cách cung cấp thông tin nhân sự và ghi nhận thưởng.
- Tách trách nhiệm quản lý, tổng hợp bảng lương vào lớp `Payroll`; sử dụng danh sách `Employee` và lời gọi đa hình thay vì phân nhánh theo từng loại nhân sự để tính lương.
- Xây dựng các tình huống kiểm thử trong `Program.cs` cho công thức thu nhập, ba cách ghi nhận thưởng, tổng bảng lương, thống kê theo phòng ban và tìm nhân sự có thu nhập cao nhất.
- Bổ sung các tình huống kiểm tra dữ liệu không hợp lệ, trùng mã nhân sự, tìm kiếm không có kết quả và bảng lương rỗng.
