/****************/
// 202419017
// Đinh Thế Vinh
/****************/ 

using System.Runtime.CompilerServices;

class Program
{
    // Tạo và hủy nhóm thứ hai trong một phạm vi cục bộ
    // NoInlining để team2 thực sự hết phạm vi khi hàm kết thúc (kể cả ở Debug)
    [MethodImpl(MethodImplOptions.NoInlining)]
    static void CreateTeam2(Employee employee)
    {
        // 13. Tạo nhóm thứ hai và thêm nhân sự đã có ở nhóm một để chứng minh kết tập nhiều nhóm
        ProjectTeam team2 = new ProjectTeam("P02", "Project Java");
        bool result13 = team2.AddMember(employee);

        Console.WriteLine("************* Kết quả *************");
        Console.WriteLine($"Thêm e1 vào team2: {result13}");
        Console.WriteLine($"e1 có trong team2: {team2.Contains(employee.Id)}");
        Console.WriteLine($"team2 đang tồn tại: {team2 != null}");
    } // team2 hết phạm vi tại đây

    // Thao tác và bắt ngoại lệ (dùng cho test biên)
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
        // 1. Tạo hai Employee bằng hai constructor khác nhau
        Console.WriteLine("1. Tạo hai Employee bằng hai constructor khác nhau");

        Employee e1 = new Employee("E01", "Nguyen Van A");
        Employee e2 = new Employee("E02", "Tran Van B", 12000);

        Console.WriteLine("************* Kết quả *************");
        e1.DisplayInfo();
        Console.WriteLine();
        e2.DisplayInfo();

        Console.WriteLine("\n----------------------------------------\n");


        // 2. Tạo hai SoftwareEngineer bằng hai constructor khác nhau
        Console.WriteLine("2. Tạo hai SoftwareEngineer bằng hai constructor khác nhau");

        SoftwareEngineer se1 = new SoftwareEngineer("SE01", "Le Van C", "C#");
        SoftwareEngineer se2 = new SoftwareEngineer("SE02", "Pham Van D", 15000, "Java", 3000);

        Console.WriteLine("************* Kết quả *************");
        se1.DisplayInfo();
        Console.WriteLine();
        se2.DisplayInfo();

        Console.WriteLine("\n----------------------------------------\n");


        // 3. Tăng lương một nhân sự bằng số tiền cố định
        Console.WriteLine("3. Tăng lương một nhân sự bằng số tiền cố định");

        Console.WriteLine($"Lương e2 trước khi tăng: {e2.BaseSalary}");
        e2.IncreaseSalary(2000);

        Console.WriteLine("************* Kết quả *************");
        Console.WriteLine($"Lương e2 sau khi tăng: {e2.BaseSalary} (kỳ vọng: 14000)");

        Console.WriteLine("\n----------------------------------------\n");


        // 4. Tăng lương một nhân sự khác theo phần trăm
        Console.WriteLine("4. Tăng lương một nhân sự khác theo phần trăm");

        Console.WriteLine($"Lương se2 trước khi tăng: {se2.BaseSalary}");
        se2.IncreaseSalary(10, true);

        Console.WriteLine("************* Kết quả *************");
        Console.WriteLine($"Lương se2 sau khi tăng 10%: {se2.BaseSalary} (kỳ vọng: 16500)");

        Console.WriteLine("\n----------------------------------------\n");


        // 5. Tạo nhóm dự án không có trưởng nhóm
        Console.WriteLine("5. Tạo nhóm dự án không có trưởng nhóm");

        ProjectTeam team1 = new ProjectTeam("P01", "Project C#");

        Console.WriteLine("************* Kết quả *************");
        team1.DisplayTeam();

        Console.WriteLine("\n----------------------------------------\n");


        // 6. Thêm một nhân sự vào nhóm bằng addMember(employee)
        Console.WriteLine("6. Thêm một nhân sự vào nhóm bằng addMember(employee)");

        bool result6 = team1.AddMember(e1);

        Console.WriteLine("************* Kết quả *************");
        Console.WriteLine($"Thêm e1: {result6}");
        team1.DisplayTeam();

        Console.WriteLine("\n----------------------------------------\n");


        // 7. Thêm một kỹ sư bằng addMember(employee, true) để đặt làm trưởng nhóm
        Console.WriteLine("7. Thêm một kỹ sư bằng addMember(employee, true) để đặt làm trưởng nhóm");

        bool result7 = team1.AddMember(se1, true);

        Console.WriteLine("************* Kết quả *************");
        Console.WriteLine($"Thêm se1: {result7}");
        Console.WriteLine($"Trưởng nhóm hiện tại: {team1.Leader.FullName}");

        Console.WriteLine("\n----------------------------------------\n");


        // 8. Thử thêm lại một thành viên đã tồn tại
        Console.WriteLine("8. Thử thêm lại một thành viên đã tồn tại");

        bool result8 = team1.AddMember(e1);

        Console.WriteLine("************* Kết quả *************");
        if (result8) Console.WriteLine("Thêm thành công.");
        else Console.WriteLine("Thêm thất bại: nhân sự đã tồn tại trong nhóm.");

        Console.WriteLine("\n----------------------------------------\n");


        // 9. Hiển thị danh sách bằng lời gọi đa hình
        Console.WriteLine("9. Hiển thị danh sách bằng lời gọi đa hình");

        Console.WriteLine("************* Kết quả *************");
        team1.DisplayTeam();

        // Minh họa đa hình: biến kiểu Employee nhưng đối tượng thật là SoftwareEngineer
        Console.WriteLine("Minh họa đa hình (Employee trỏ tới SoftwareEngineer):");
        Employee polymorphic = se1;
        polymorphic.DisplayInfo();
        Console.WriteLine($"Chi phí hàng tháng (gọi qua Employee): {polymorphic.CalculateMonthlyCost()}");

        Console.WriteLine("\n----------------------------------------\n");


        // 10. Tính tổng chi phí nhân sự hàng tháng
        Console.WriteLine("10. Tính tổng chi phí nhân sự hàng tháng");

        double totalCost = team1.CalculateTotalMonthlyCost();

        Console.WriteLine("************* Kết quả *************");
        Console.WriteLine($"Tổng chi phí hàng tháng: {totalCost}");

        Console.WriteLine("\n----------------------------------------\n");


        // 11. Thử xóa trưởng nhóm hiện tại và kiểm tra thao tác bị từ chối
        Console.WriteLine("11. Thử xóa trưởng nhóm hiện tại và kiểm tra thao tác bị từ chối");


        Console.WriteLine("************* Kết quả *************");

        bool result11 = team1.RemoveMember(se1.Id);

        if (result11) Console.WriteLine("Xóa thành công.");
        else Console.WriteLine("Xóa thất bại: không thể xóa trưởng nhóm hiện tại.");

        Console.WriteLine($"Trưởng nhóm hiện tại vẫn là: {team1.Leader.FullName}");
        Console.WriteLine($"se1 vẫn là thành viên: {team1.Contains(se1.Id)}");

        Console.WriteLine("\n----------------------------------------\n");


        // 12. Đổi trưởng nhóm rồi xóa người từng là trưởng nhóm
        Console.WriteLine("12. Đổi trưởng nhóm rồi xóa người từng là trưởng nhóm");

        Console.WriteLine("************* Kết quả *************");

        team1.ChangeLeader(e1);
        Console.WriteLine($"Trưởng nhóm mới: {team1.Leader.FullName}");

        bool result12 = team1.RemoveMember(se1.Id);
        Console.WriteLine($"Xóa se1: {result12}");
        Console.WriteLine($"Trưởng nhóm hiện tại: {team1.Leader.FullName}");
        Console.WriteLine($"se1 còn trong nhóm: {team1.Contains(se1.Id)}");

        Console.WriteLine("Danh sách nhóm sau khi xóa:");
        team1.DisplayTeam();

        Console.WriteLine("\n----------------------------------------\n");


        // 13 + 14. Tạo nhóm thứ hai trong phạm vi cục bộ, hết phạm vi thì nhóm bị hủy
        Console.WriteLine("13. Tạo một nhóm thứ hai và thêm một nhân sự đã có ở nhóm thứ nhất để chứng minh quan hệ kết tập nhiều nhóm");

        CreateTeam2(e1);
        Console.WriteLine($"e1 có trong team1: {team1.Contains(e1.Id)}");

        Console.WriteLine("\n----------------------------------------\n");


        // 14. Hủy nhóm thứ hai bằng cách kết thúc một khối lệnh cục bộ
        Console.WriteLine("14. Hủy nhóm thứ hai bằng cách kết thúc một khối lệnh cục bộ");

        Console.WriteLine("************* Kết quả *************");
        Console.WriteLine("team2 đã hết phạm vi, ép GC thu gom để quan sát destructor:");

        GC.Collect();
        GC.WaitForPendingFinalizers();

        Console.WriteLine("\n----------------------------------------\n");

        // 15. Chứng minh nhân sự của nhóm thứ hai vẫn tồn tại sau khi nhóm bị hủy
        Console.WriteLine();
        Console.WriteLine("15. Chứng minh nhân sự của nhóm thứ hai vẫn tồn tại sau khi nhóm bị hủy");

        Console.WriteLine("************* Kết quả *************");
        e1.DisplayInfo();
        Console.WriteLine($"e1 vẫn còn trong team1: {team1.Contains(e1.Id)}");

        Console.WriteLine("\n----------------------------------------\n");


        // 16. Kiểm thử các trường hợp biên
        Console.WriteLine("16. Kiểm thử các trường hợp biên");

        Console.WriteLine("************* Kết quả *************");

        TestEdge("Employee với id rỗng", () => new Employee("", "abc"));
        TestEdge("Employee với họ tên rỗng", () => new Employee("E03", " "));
        TestEdge("Employee với lương âm", () => new Employee("E03", "abc", -100));
        TestEdge("Tăng lương 0", () => e2.IncreaseSalary(0));
        TestEdge("Tăng lương số âm", () => e2.IncreaseSalary(-500));
        TestEdge("Tăng phần trăm số âm", () => e2.IncreaseSalary(-5, true));
        TestEdge("SoftwareEngineer với ngôn ngữ rỗng", () => new SoftwareEngineer("SE03", "abc", ""));
        TestEdge("SoftwareEngineer với phụ cấp âm", () => new SoftwareEngineer("SE03", "abc", 1000, "C#", -1));
        TestEdge("ProjectTeam với mã dự án rỗng", () => new ProjectTeam("", "abc"));
        TestEdge("ProjectTeam với trưởng nhóm null", () => new ProjectTeam("P03", "abc", null));
        TestEdge("AddMember(null)", () => team1.AddMember(null));
        TestEdge("ChangeLeader(null)", () => team1.ChangeLeader(null));

        Console.WriteLine($"Xóa id không tồn tại: {team1.RemoveMember("NOT_EXIST")}");

        Console.WriteLine("\n----------------------------------------\n");
    }
}
