/****************/
// 202419017
// Đinh Thế Vinh
/****************/ 

class Program
{
    // Bắt lỗi, ngoại lệ
    static void TestEdge(string description, Action action)
    {
        try
        {
            action();
            Console.WriteLine($"[KHÔNG lỗi] {description}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"[Bắt được lỗi] {description} -> {ex.Message}");
        }
    }

    static void Main()
    {
        // 1. Tạo Employee bằng hai constructor khác nhau
        Console.WriteLine("1. Tạo Employee bằng hai constructor khác nhau");

        Employee employee1 = new Employee("E001", "Nguyễn Minh An");
        Employee employee2 = new Employee("E005", "Trần Văn Test", "IT");

        Console.WriteLine("************* Kết quả *************");
        employee1.DisplayPayrollInfo();
        Console.WriteLine();
        employee2.DisplayPayrollInfo();

        Console.WriteLine("\n----------------------------------------\n");


        // 2. Tạo SalariedEmployee
        Console.WriteLine("2. Tạo SalariedEmployee");

        SalariedEmployee e1 = new SalariedEmployee("E001", "Nguyễn Minh An", "Đào tạo", 15000000, 2000000);
        e1.AddBonus(1000000);

        Console.WriteLine("************* Kết quả *************");
        e1.DisplayPayrollInfo();
        Console.WriteLine($"Thu nhập kỳ vọng: 18000000");

        Console.WriteLine("\n----------------------------------------\n");


        // 3. Tạo HourlyEmployee không có giờ vượt ngưỡng
        Console.WriteLine("3. Tạo HourlyEmployee không có giờ vượt ngưỡng");

        HourlyEmployee e2 = new HourlyEmployee("E002", "Trần Thu Bình", "Hỗ trợ", 100000, 150);
        e2.AddBonus(500000);

        Console.WriteLine("************* Kết quả *************");
        e2.DisplayPayrollInfo();
        Console.WriteLine($"Thu nhập kỳ vọng: 15500000");

        Console.WriteLine("\n----------------------------------------\n");


        // 4. Tạo HourlyEmployee có giờ vượt ngưỡng 160
        Console.WriteLine("4. Tạo HourlyEmployee có giờ vượt ngưỡng 160");

        HourlyEmployee e3 = new HourlyEmployee("E003", "Lê Hoàng Chi", "Hỗ trợ", 100000, 170);

        Console.WriteLine("************* Kết quả *************");
        e3.DisplayPayrollInfo();
        Console.WriteLine($"Thu nhập kỳ vọng: 17500000");

        Console.WriteLine("\n----------------------------------------\n");


        // 5. Tạo SalesEmployee
        Console.WriteLine("5. Tạo SalesEmployee");

        SalesEmployee e4 = new SalesEmployee("E004", "Phạm Quốc Dũng", "Kinh doanh", 8000000, 200000000, 0.05);
        e4.AddBonus(0.02, 50000000, "Thưởng theo doanh số");

        Console.WriteLine("************* Kết quả *************");
        e4.DisplayPayrollInfo();
        Console.WriteLine($"Thu nhập kỳ vọng: 19000000");

        Console.WriteLine("\n----------------------------------------\n");


        // 6. Kiểm tra ba phiên bản AddBonus()
        Console.WriteLine("6. Kiểm tra ba phiên bản AddBonus()");

        Employee bonusTest = new Employee("E100", "Bonus Test", "Test");

        bonusTest.AddBonus(500000);
        bonusTest.AddBonus(500000, "Thưởng hoàn thành công việc");
        bonusTest.AddBonus(0.02, 50000000, "Thưởng theo doanh số");

        Console.WriteLine("************* Kết quả *************");
        Console.WriteLine($"Monthly Bonus: {bonusTest.MonthlyBonus}");
        Console.WriteLine("Kỳ vọng: 2000000");

        Console.WriteLine("\n----------------------------------------\n");


        // 7. Tạo Payroll và thêm nhân viên
        Console.WriteLine("7. Tạo Payroll và thêm nhân viên");

        Payroll payroll = new Payroll("10/2026");

        payroll.AddEmployee(e1);
        payroll.AddEmployee(e2);
        payroll.AddEmployee(e3);
        payroll.AddEmployee(e4);

        Console.WriteLine("************* Kết quả *************");
        Console.WriteLine("Đã thêm E001, E002, E003, E004 vào Payroll.");

        Console.WriteLine("\n----------------------------------------\n");


        // 8. Tìm nhân viên theo Employee ID
        Console.WriteLine("8. Tìm nhân viên theo Employee ID");

        Employee? foundEmployee = payroll.FindEmployee("E003");

        Console.WriteLine("************* Kết quả *************");

        if (foundEmployee != null)
        {
            Console.WriteLine($"Tìm thấy: {foundEmployee.EmployeeId} - {foundEmployee.FullName}");
            foundEmployee.DisplayPayrollInfo();
        }
        else
        {
            Console.WriteLine("Không tìm thấy nhân viên.");
        }

        Console.WriteLine("\n----------------------------------------\n");


        // 9. Tính tổng bảng lương
        Console.WriteLine("9. Tính tổng bảng lương");

        double totalPayroll = payroll.CalculateTotalPayroll();

        Console.WriteLine("************* Kết quả *************");
        Console.WriteLine($"Tổng bảng lương: {totalPayroll:N0}");
        Console.WriteLine("Kỳ vọng: 70,000,000");

        Console.WriteLine("\n----------------------------------------\n");


        // 10. Tính tổng bảng lương theo phòng ban
        Console.WriteLine("10. Tính tổng bảng lương theo phòng ban");

        double supportPayroll = payroll.CalculatePayrollByDepartment("Hỗ trợ");

        Console.WriteLine("************* Kết quả *************");
        Console.WriteLine($"Tổng bảng lương phòng Hỗ trợ: {supportPayroll:N0}");
        Console.WriteLine("Kỳ vọng: 33,000,000");

        Console.WriteLine("\n----------------------------------------\n");


        // 11. Tìm nhân viên có thu nhập cao nhất
        Console.WriteLine("11. Tìm nhân viên có thu nhập cao nhất");

        Employee? highestPaid = payroll.FindHighestPaidEmployee();

        Console.WriteLine("************* Kết quả *************");

        if (highestPaid != null)
        {
            Console.WriteLine($"Nhân viên có thu nhập cao nhất: {highestPaid.FullName}");
            Console.WriteLine($"Employee ID: {highestPaid.EmployeeId}");
            Console.WriteLine($"Gross Pay: {highestPaid.CalculateGrossPay():N0}");
            Console.WriteLine("Kỳ vọng: E004 - Phạm Quốc Dũng - 19,000,000");
        }
        else
        {
            Console.WriteLine("Danh sách nhân viên đang rỗng.");
        }

        Console.WriteLine("\n----------------------------------------\n");


        // 12. Hiển thị toàn bộ bảng lương bằng đa hình
        Console.WriteLine("12. Hiển thị toàn bộ bảng lương bằng đa hình");

        Console.WriteLine("************* Kết quả *************");
        payroll.DisplayPayroll();

        Console.WriteLine("\n----------------------------------------\n");


        // 13. Minh họa đa hình
        Console.WriteLine("13. Minh họa đa hình");

        Employee polymorphic = e3;

        Console.WriteLine("************* Kết quả *************");
        Console.WriteLine($"Biến có kiểu Employee nhưng đối tượng thật là: {polymorphic.GetEmployeeType()}");
        Console.WriteLine($"Gross Pay: {polymorphic.CalculateGrossPay():N0}");

        Console.WriteLine("\n----------------------------------------\n");


        // 14. Cập nhật doanh số bằng phương thức có kiểm soát
        Console.WriteLine("14. Cập nhật doanh số của SalesEmployee");

        Console.WriteLine($"Doanh số trước khi cập nhật: {e4.SalesRevenue:N0}");

        e4.UpdateSalesRevenue(250000000);

        Console.WriteLine("************* Kết quả *************");
        Console.WriteLine($"Doanh số sau khi cập nhật: {e4.SalesRevenue:N0}");
        Console.WriteLine($"Gross Pay mới: {e4.CalculateGrossPay():N0}");

        Console.WriteLine("\n----------------------------------------\n");


        // 15. Thử thêm nhân viên bị trùng Employee ID
        Console.WriteLine("15. Thử thêm nhân viên bị trùng Employee ID");

        TestEdge("Thêm nhân viên E001 bị trùng", () =>
        {
            payroll.AddEmployee(new Employee("E001", "Nhân viên trùng mã"));
        });

        Console.WriteLine("\n----------------------------------------\n");


        // 16. Kiểm thử các trường hợp biên
        Console.WriteLine("16. Kiểm thử các trường hợp biên");

        Console.WriteLine("************* Kết quả *************");

        TestEdge("Employee với ID rỗng", () =>
            new Employee("", "Nguyễn Văn A"));

        TestEdge("Employee với Full Name rỗng", () =>
            new Employee("E101", " "));

        TestEdge("Employee với Department rỗng", () =>
            new Employee("E102", "Nguyễn Văn B", ""));

        TestEdge("Employee với Monthly Bonus âm", () =>
        {
            Employee test = new Employee("E103", "Nguyễn Văn C");
            test.MonthlyBonus = -100000;
        });

        TestEdge("HourlyEmployee với Hourly Rate âm", () =>
            new HourlyEmployee("E104", "Test", "Hỗ trợ", -100000, 100));

        TestEdge("HourlyEmployee với Worked Hours âm", () =>
            new HourlyEmployee("E105", "Test", "Hỗ trợ", 100000, -1));

        TestEdge("HourlyEmployee với Worked Hours > 250", () =>
            new HourlyEmployee("E106", "Test", "Hỗ trợ", 100000, 251));

        TestEdge("SalariedEmployee với Monthly Salary âm", () =>
            new SalariedEmployee("E107", "Test", "Đào tạo", -1000000, 0));

        TestEdge("SalariedEmployee với Responsibility Allowance âm", () =>
            new SalariedEmployee("E108", "Test", "Đào tạo", 1000000, -100000));

        TestEdge("SalesEmployee với Base Salary âm", () =>
            new SalesEmployee("E109", "Test", "Kinh doanh", -1000000, 10000000, 0.05));

        TestEdge("SalesEmployee với Sales Revenue âm", () =>
            new SalesEmployee("E110", "Test", "Kinh doanh", 1000000, -10000000, 0.05));

        TestEdge("SalesEmployee với Commission Rate > 0.3", () =>
            new SalesEmployee("E111", "Test", "Kinh doanh", 1000000, 10000000, 0.31));

        TestEdge("AddBonus với amount = 0", () =>
            new Employee("E112", "Test").AddBonus(0));

        TestEdge("AddBonus với amount âm", () =>
            new Employee("E113", "Test").AddBonus(-100000));

        TestEdge("AddBonus có reason rỗng", () =>
            new Employee("E114", "Test").AddBonus(100000, ""));

        TestEdge("AddBonus theo rate > 0.5", () =>
            new Employee("E115", "Test").AddBonus(0.6, 1000000, "Test"));

        TestEdge("AddBonus với referenceAmount <= 0", () =>
            new Employee("E116", "Test").AddBonus(0.1, 0, "Test"));

        TestEdge("AddBonus theo rate có reason rỗng", () =>
            new Employee("E117", "Test").AddBonus(0.1, 1000000, ""));

        TestEdge("Payroll với Period rỗng", () =>
            new Payroll(""));

        Console.WriteLine("\n----------------------------------------\n");


        // 17. Tìm nhân viên không tồn tại
        Console.WriteLine("17. Tìm nhân viên không tồn tại");

        Employee? notFound = payroll.FindEmployee("NOT_EXIST");

        Console.WriteLine("************* Kết quả *************");

        if (notFound == null)
            Console.WriteLine("Không tìm thấy nhân viên với ID NOT_EXIST.");
        else
            Console.WriteLine($"Tìm thấy: {notFound.FullName}");

        Console.WriteLine("\n----------------------------------------\n");


        // 18. Kiểm tra Payroll rỗng
        Console.WriteLine("18. Kiểm tra Payroll rỗng");

        Payroll emptyPayroll = new Payroll("11/2026");
        Employee? highestInEmptyPayroll = emptyPayroll.FindHighestPaidEmployee();

        Console.WriteLine("************* Kết quả *************");

        if (highestInEmptyPayroll == null)
            Console.WriteLine("Payroll rỗng -> không có nhân viên có thu nhập cao nhất.");
        else
            Console.WriteLine($"Nhân viên cao nhất: {highestInEmptyPayroll.FullName}");

        Console.WriteLine("\n----------------------------------------\n");


        Console.WriteLine("Đã hoàn thành toàn bộ chương trình kiểm thử.");
    }
}