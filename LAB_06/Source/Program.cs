/****************/
// 202419017
// Đinh Thế Vinh
/****************/
using System;
using Source.Enum;
using Source.Interfaces;
using Source.Models.Borrowing;
using Source.Models.Devices;
using Source.Models.People;

class Program
{
    // Bắt lỗi, ngoại lệ
    static void TestEdge(string description, Action action)
    {
        try
        {
            action();
            Console.WriteLine($"[KHÔNG phát sinh lỗi] {description}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Bắt được ngoại lệ] {description} -> {ex.GetType().Name}: {ex.Message}");
        }
    }

    static void Main()
    {
        Console.WriteLine("========== 1. LUỒNG MƯỢN TRẢ THÀNH CÔNG ==========\n");

        DateTime borrowedDate = DateTime.Today.AddDays(-5);

        // 1. Khởi tạo người mượn, thiết bị và phiếu mượn
        Console.WriteLine("1. Khởi tạo người mượn, thiết bị và phiếu mượn");
        Lecturer lecturer = new("GV01", "Dinh The Vinh");
        ProjectorRemote projector = new("PR01", "Dieu khien may chieu", 1);
        Micro micro = new("MI01", "Micro khong day", 1);
        AirConditionerRemote airConditioner = new("AC01", "Dieu khien dieu hoa", 1);
        BorrowingSlip slip = new("SL01", lecturer, "Phong A101", borrowedDate);

        // 2. Mượn hai thiết bị và kiểm tra trạng thái sau khi mượn
        Console.WriteLine("\n2. Mượn hai thiết bị và kiểm tra trạng thái sau khi mượn");
        slip.AddDevice(projector, 1);
        slip.AddDevice(micro, 1);
        Console.WriteLine($"PR01 sau khi mượn: {projector.Status}");
        Console.WriteLine($"MI01 sau khi mượn: {micro.Status}");
        Console.WriteLine($"Ma phieu: {slip.SlipId}");
        Console.WriteLine($"Nguoi muon: {slip.Borrower.FullName}");
        Console.WriteLine($"Phong hoc: {slip.TeachingRoom}");
        Console.WriteLine($"Ngay muon: {slip.BorrowedDate:dd/MM/yyyy}");
        Console.WriteLine("Da muon 2 thiet bi");

        // 3. Trả điều khiển máy chiếu đúng hạn và tính phí
        Console.WriteLine("\n3. Trả điều khiển máy chiếu đúng hạn và tính phí");
        decimal projectorFee = slip.ReturnDevice(projector.DeviceId, borrowedDate.AddDays(1));
        Console.WriteLine($"Tra dieu khien may chieu dung han. Phi: {projectorFee:N0} VND");

        // 4. Trả micro muộn hai ngày và tính phí trả muộn
        Console.WriteLine("\n4. Trả micro muộn hai ngày và tính phí trả muộn");
        decimal microFee = slip.ReturnDevice(micro.DeviceId, borrowedDate.AddDays(3));
        Console.WriteLine($"Tra micro muon 2 ngay. Phi: {microFee:N0} VND");
        Console.WriteLine($"Tong phi tra muon: {slip.CalculateTotalLateFee():N0} VND");

        // 5. Kiểm tra danh sách thiết bị trả muộn hoặc chưa trả
        Console.WriteLine("\n5. Kiểm tra danh sách thiết bị trả muộn hoặc chưa trả");
        Console.WriteLine("Danh sach thiet bi tra muon hoac chua tra:");
        foreach (BorrowItem item in slip.Items)
        {
            int lateDays = item.IsReturned ? (item.ReturnedDate.Date - item.DueDate.Date).Days : 0;

            if (!item.IsReturned || lateDays > 0)
            {
                Console.WriteLine($"- {item.Device.Name}: " + (item.IsReturned ? $"Da tra muon {lateDays} ngay" : "Chua tra"));
            }
        }

        Console.WriteLine("\n========== 2. KIỂM THỬ NGOẠI LỆ ==========\n");

        // 1. Mượn thiết bị đang bảo trì
        Console.WriteLine("\n1. Mượn thiết bị đang bảo trì");
        TestEdge("Muon dieu khien dieu hoa dang UnderMaintenance", () =>
        {
            AirConditionerRemote ac = new("AC02", "Dieu khien dieu hoa", 1);
            ac.Status = DeviceStatus.UnderMaintenance;
            BorrowingSlip testSlip = new("SL02", new Lecturer("GV02", "Tran Van Binh"), "Phong A102", DateTime.Today);
            testSlip.AddDevice(ac, 1);
        });

        // 2. Trợ giảng mượn vượt quá số thiết bị cho phép
        Console.WriteLine("\n2. Trợ giảng mượn vượt quá số thiết bị cho phép");
        TestEdge("Tro giang muon thiet bi thu ba", () =>
        {
            TeachingAssistant assistant = new("TG01", "Dinh Minh Duc");
            BorrowingSlip testSlip = new("SL03", assistant, "Phong A103", DateTime.Today);
            testSlip.AddDevice(new ProjectorRemote("PR02", "Dieu khien may chieu", 1), 1);
            testSlip.AddDevice(new Micro("MI02", "Micro", 1), 1);
            testSlip.AddDevice(new AirConditionerRemote("AC03", "Dieu khien dieu hoa", 1), 1);
        });

        // 3. Thêm trùng một thiết bị vào cùng phiếu mượn
        Console.WriteLine("\n3. Thêm trùng một thiết bị vào cùng phiếu mượn");
        TestEdge("Them cung mot micro hai lan", () =>
        {
            Micro testMicro = new("MI03", "Micro", 1);
            BorrowingSlip testSlip = new("SL04", new Lecturer("GV03", "Pham Van Nam"), "Phong A104", DateTime.Today);
            testSlip.AddDevice(testMicro, 1);
            testSlip.AddDevice(testMicro, 1);
        });

        // 4. Thời gian mượn vượt quá giới hạn thiết bị
        Console.WriteLine("\n4. Thời gian mượn vượt quá giới hạn thiết bị");
        TestEdge("Muon micro 3 ngay khi gioi han la 1 ngay", () =>
        {
            Micro testMicro = new("MI04", "Micro", 1);
            BorrowingSlip testSlip = new("SL05", new Lecturer("GV04", "Do Van Hung"), "Phong A105", DateTime.Today);
            testSlip.AddDevice(testMicro, 3);
        });

        // 5. Trả thiết bị không có trong phiếu mượn
        Console.WriteLine("\n5. Trả thiết bị không có trong phiếu mượn");
        TestEdge("Tra dieu khien may chieu khong thuoc phieu", () =>
        {
            BorrowingSlip testSlip = new("SL06", new Lecturer("GV05", "Hoang Van Long"), "Phong A106", DateTime.Today);
            testSlip.ReturnDevice("PR99", DateTime.Today);
        });

        // 6. Trả cùng một thiết bị nhiều lần
        Console.WriteLine("\n6. Trả cùng một thiết bị nhiều lần");
        TestEdge("Tra cung mot thiet bi hai lan", () =>
        {
            DateTime date = DateTime.Today;
            ProjectorRemote testProjector = new("PR03", "Dieu khien may chieu", 1);
            BorrowingSlip testSlip = new("SL07", new Lecturer("GV06", "Vu Van Duc"), "Phong A107", date);
            testSlip.AddDevice(testProjector, 1);
            testSlip.ReturnDevice(testProjector.DeviceId, date.AddDays(1));
            testSlip.ReturnDevice(testProjector.DeviceId, date.AddDays(1));
        });

        // 7. Trả thiết bị trước ngày mượn
        Console.WriteLine("\n7. Trả thiết bị trước ngày mượn ");
        TestEdge("Tra thiet bi truoc ngay muon", () =>
        {
            DateTime date = DateTime.Today;
            ProjectorRemote testProjector = new("PR04", "Dieu khien may chieu", 1);
            BorrowingSlip testSlip = new("SL08", new Lecturer("GV07", "Bui Van Son"), "Phong A108", date);
            testSlip.AddDevice(testProjector, 1);
            testSlip.ReturnDevice(testProjector.DeviceId, date.AddDays(-1));
        });

        Console.WriteLine("\n========== KET THUC KIEM THU ==========");
    }
}