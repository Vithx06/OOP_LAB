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

## 5. Trả lời câu hỏi thiết kế

### 5.1. Vì sao CalculateGrossPay() phù hợp cho ghi đè, không phải nạp chồng?

Các loại nhân sự cùng có thao tác tính thu nhập không nhận tham số, nhưng công thức phụ thuộc vào loại đối tượng thực tế. Khai báo `virtual` ở `Employee` và `override` ở các lớp dẫn xuất cho phép gọi đúng công thức qua biến kiểu `Employee` khi chạy. Nạp chồng theo danh sách tham số không giải quyết việc chọn công thức theo loại nhân sự như yêu cầu này.

### 5.2. Vì sao AddBonus() phù hợp cho nạp chồng?

Các phiên bản cùng thực hiện việc cộng thưởng nhưng nhận dữ liệu theo ba cách: số tiền, số tiền kèm lý do, hoặc tỷ lệ cùng giá trị tham chiếu và lý do. Nạp chồng giữ cùng tên thao tác và phân biệt các cách gọi qua số lượng, kiểu tham số. Với các lời gọi thông thường trong bài, trình biên dịch chọn phiên bản dựa trên kiểu đối số, không chọn theo loại nhân sự lúc chạy.

### 5.3. Payroll nên lưu đối tượng như thế nào để giữ hành vi lớp dẫn xuất?

Trong C#, các lớp nhân sự là kiểu tham chiếu, nên `List<Employee>` lưu các tham chiếu đến đối tượng thực tế, bao gồm `SalariedEmployee`, `HourlyEmployee` và `SalesEmployee`. Việc đưa chúng vào danh sách không cắt mất phần dữ liệu của lớp dẫn xuất. Khi `Payroll` gọi `CalculateGrossPay()` qua `Employee`, cơ chế gọi phương thức ảo chọn bản ghi đè của đối tượng thực tế; không cần con trỏ hay phân nhánh theo từng lớp.

### 5.4. Employee nên là lớp cụ thể hay lớp trừu tượng?

Mã hiện tại chọn lớp cụ thể để có thể khởi tạo `Employee` và minh họa constructor, nạp chồng thưởng như trong `Program.cs`; thu nhập mặc định của lớp này chỉ bằng `MonthlyBonus`. Lựa chọn này đơn giản cho bài thực hành nhưng cho phép tồn tại nhân sự chưa có cơ chế lương thuộc một trong ba loại nghiệp vụ. Nếu hệ thống chỉ chấp nhận ba loại đó, nên dùng lớp `abstract` và buộc mỗi loại triển khai công thức tính lương, đồng thời sửa các chỗ đang trực tiếp tạo `Employee`.

### 5.5. Nếu nhân viên kinh doanh cũng được trả theo giờ, kế thừa đơn có còn phù hợp không?

Kế thừa đơn vẫn dùng được nếu tạo một lớp nhân sự có công thức kết hợp, nhưng việc thêm lớp cho mọi tổ hợp chính sách lương sẽ khó mở rộng. C# không hỗ trợ kế thừa đồng thời từ cả `SalesEmployee` và `HourlyEmployee`. Có thể tách cách tính tiền công theo giờ và hoa hồng thành các thành phần tính lương rồi kết hợp chúng; đổi lại cần thêm abstraction và cấu hình các thành phần đó.

### 5.6. Có nên đặt CalculateTotalPayroll() là static không?

Không nên với thiết kế hiện tại vì mỗi `Payroll` có kỳ lương và danh sách nhân sự riêng, còn phương thức cần tổng hợp chính danh sách đó. Phương thức instance thể hiện rõ tổng thu nhập thuộc bảng lương nào. Một hàm `static` nhận danh sách từ bên ngoài vẫn có thể tính tổng, nhưng không cần thiết cho trách nhiệm đang có của `Payroll`.

### 5.7. Vì sao chỉ lưu tổng thưởng thay vì lịch sử từng khoản thưởng?

Mã hiện tại chỉ cộng các khoản hợp lệ vào `MonthlyBonus` vì mục tiêu là tính tổng thu nhập trong kỳ. `reason` được kiểm tra ở các phiên bản yêu cầu lý do nhưng không được lưu lại. Cách này ít dữ liệu và đơn giản, đổi lại không truy vết hoặc hoàn tác riêng từng khoản; nếu cần kiểm toán, nên thêm lớp bản ghi thưởng và một danh sách bản ghi, không dùng nhiều mảng song song.

### 5.8. Phiên bản AddBonus() nào được chọn trong từng cách gọi?

- `AddBonus(1000000)` và `AddBonus(500000)` chọn `AddBonus(double amount)` vì chỉ truyền số tiền.
- `AddBonus(500000, "Thưởng hoàn thành công việc")` chọn `AddBonus(double amount, string reason)` vì có số tiền và lý do.
- `AddBonus(0.02, 50000000, "Thưởng theo doanh số")` chọn `AddBonus(double rate, double referenceAmount, string reason)` vì có tỷ lệ, giá trị tham chiếu và lý do; tiền thưởng là 1.000.000.

Các số nguyên trong những lời gọi trên được chuyển ngầm định sang `double`. Kể cả lời gọi có dữ liệu không hợp lệ, như `AddBonus(0)`, phiên bản vẫn được chọn khi biên dịch; lỗi giá trị chỉ được phát hiện khi thân phương thức chạy.

### 5.9. Việc chọn CalculateGrossPay() của lớp dẫn xuất diễn ra như thế nào?

Với `Employee polymorphic = e3`, biến có kiểu khai báo `Employee` nhưng đối tượng thực tế là `HourlyEmployee`. Khi gọi `polymorphic.CalculateGrossPay()`, bản ghi đè của `HourlyEmployee` được chọn lúc chạy; công thức tính cả giờ vượt ngưỡng 160 nếu có. Các vòng lặp trong `Payroll` sử dụng cùng cơ chế để tổng hợp lương mà không cần kiểm tra loại nhân sự.

### 5.10. Nếu thay List<Employee> bằng nhiều danh sách riêng cho từng loại thì sao?

Mỗi danh sách riêng giúp truy cập trực tiếp thông tin đặc thù của một loại, nhưng việc thêm, tìm, hiển thị và tổng hợp toàn bộ nhân sự phải xử lý qua nhiều danh sách. Khi có loại mới, nhiều chỗ tổng hợp có thể phải sửa theo. `List<Employee>` giữ một luồng xử lý chung, còn khác biệt công thức được giao cho phương thức ghi đè.

### 5.11. Khi thêm ContractEmployee, phần mã nào cần sửa?

Cần thêm lớp `ContractEmployee` kế thừa `Employee`, khai báo dữ liệu hợp đồng, kiểm tra đầu vào và ghi đè các hành vi tính lương, nhận diện loại và hiển thị. Cần bổ sung nơi khởi tạo, dữ liệu kiểm thử và cập nhật UML. Các thuật toán tổng hợp của `Payroll` không cần sửa nếu lớp mới tuân theo hợp đồng `Employee` và được thêm vào danh sách chung.
