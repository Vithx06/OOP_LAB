/****************/
// 202419017
// Đinh Thế Vinh
/****************/

using System;
using System.Collections.Generic;
using System.Linq;
using Source.Enum;
using Source.Exceptions;
using Source.Interfaces;
using Source.Models.Devices;
using Source.Models.Services;
using Source.Models.Strategies;
using Source.Models.Repositories;

class Program
{
    // Bắt lỗi, ngoại lệ
    static void TestEdge(string description, Action action)
    {
        Console.WriteLine($"\n{description}");

        try
        {
            action();
            Console.WriteLine("[KHÔNG phát sinh lỗi]");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Bắt được ngoại lệ] {ex.GetType().Name}: {ex.Message}");
        }

        Console.WriteLine();
    }

    static void Main(string[] args)
    {
        IRepository<ProjectorRemote> projectorRepository = new InMemoryRepository<ProjectorRemote>();
        IRepository<AirConditionerRemote> airConditionerRepository = new InMemoryRepository<AirConditionerRemote>();
        IRepository<WirelessMicrophone> microphoneRepository = new InMemoryRepository<WirelessMicrophone>();

        // Khởi tạo các thiết bị để kiểm thử
        ProjectorRemote projector1 = new ProjectorRemote("PR01", "Điều khiển máy chiếu 1", "Tủ A", 40);
        ProjectorRemote projector2 = new ProjectorRemote("PR02", "Điều khiển máy chiếu 2", "Tủ A", 85);
        ProjectorRemote projector3 = new ProjectorRemote("PR03", "Điều khiển máy chiếu 3", "Tủ B", 15);

        AirConditionerRemote airConditioner1 = new AirConditionerRemote("AC01", "Điều khiển điều hòa 1", "Tủ A", new HashSet<string> { "A203", "A204" });
        AirConditionerRemote airConditioner2 = new AirConditionerRemote("AC02", "Điều khiển điều hòa 2", "Tủ B", new HashSet<string> { "B101", "B102" });

        WirelessMicrophone microphone1 = new WirelessMicrophone("MI01", "Micro không dây 1", "Tủ A", 90, 80);
        WirelessMicrophone microphone2 = new WirelessMicrophone("MI02", "Micro không dây 2", "Tủ A", 60, 70);
        WirelessMicrophone microphone3 = new WirelessMicrophone("MI03", "Micro không dây 3", "Tủ B", 20, 30);

        // 1. Thêm thiết bị vào repository
        TestEdge("1. Thêm thiết bị vào repository", () =>
        {
            projectorRepository.Add(projector1);
            projectorRepository.Add(projector2);
            projectorRepository.Add(projector3);

            airConditionerRepository.Add(airConditioner1);
            airConditionerRepository.Add(airConditioner2);

            microphoneRepository.Add(microphone1);
            microphoneRepository.Add(microphone2);
            microphoneRepository.Add(microphone3);

            Console.WriteLine($"Số điều khiển máy chiếu: {projectorRepository.GetAll().Count}");
            Console.WriteLine($"Số điều khiển điều hòa: {airConditionerRepository.GetAll().Count}");
            Console.WriteLine($"Số micro không dây: {microphoneRepository.GetAll().Count}");
        });

        // 2. Thử thêm hai thiết bị có cùng mã
        TestEdge("2. Thử thêm hai thiết bị có cùng mã", () =>
        {
            ProjectorRemote duplicateProjector = new ProjectorRemote("PR01", "Điều khiển máy chiếu trùng lặp", "Tủ C", 70);
            projectorRepository.Add(duplicateProjector);
        });

        // 3. Tìm thiết bị theo mã
        TestEdge("3. Tìm thiết bị theo mã", () =>
        {
            ProjectorRemote? found = projectorRepository.FindById("PR02");
            ProjectorRemote? notFound = projectorRepository.FindById("PR99");

            Console.WriteLine($"Tìm PR02: {(found != null ? found.Name : "Không tìm thấy")}");
            Console.WriteLine($"Tìm PR99: {(notFound != null ? notFound.Name : "Không tìm thấy")}");
        });

        // 4. Lọc các thiết bị sẵn sàng sử dụng
        TestEdge("4. Lọc các thiết bị sẵn sàng sử dụng", () =>
        {
            IReadOnlyList<ProjectorRemote> readyProjectors = projectorRepository.FindAll(device => device.IsReadyForUse());
            IReadOnlyList<WirelessMicrophone> readyMicrophones = microphoneRepository.FindAll(device => device.IsReadyForUse());

            Console.WriteLine("Điều khiển máy chiếu sẵn sàng:");
            foreach (ProjectorRemote device in readyProjectors)
            {
                Console.WriteLine($"{device.Id} - {device.Name} - Pin: {device.BatteryLevel}");
            }

            Console.WriteLine("Micro không dây sẵn sàng:");
            foreach (WirelessMicrophone device in readyMicrophones)
            {
                Console.WriteLine($"{device.Id} - {device.Name}");
            }
        });

        // 5. Chọn điều khiển máy chiếu có điểm ưu tiên cao nhất (Giả sử phòng cần mượn là A203)
        TestEdge("5. Chọn điều khiển máy chiếu có điểm ưu tiên cao nhất", () =>
        {
            string roomId = "A203";

            HighestPriorityStrategy<ProjectorRemote> strategy = new HighestPriorityStrategy<ProjectorRemote>(device => device.GetDefaultPriorityScore(roomId));
            AllocationService<ProjectorRemote> service = new AllocationService<ProjectorRemote>(projectorRepository, strategy);

            ProjectorRemote selected = service.Allocate(roomId, device => true);

            Console.WriteLine($"Thiết bị được chọn: {selected.Id} - {selected.Name}");
            Console.WriteLine($"Điểm ưu tiên: {selected.GetDefaultPriorityScore(roomId)}");
        });

        // 6. Chọn điều khiển điều hòa tương thích với phòng
        TestEdge("6. Chọn điều khiển điều hòa tương thích với phòng", () =>
        {
            FirstAvailableStrategy<AirConditionerRemote> strategy = new FirstAvailableStrategy<AirConditionerRemote>();
            AllocationService<AirConditionerRemote> service = new AllocationService<AirConditionerRemote>(airConditionerRepository, strategy);

            string roomId = "A203";
            AirConditionerRemote selected = service.Allocate(roomId, device => device.IsCompatibleWith(roomId));

            Console.WriteLine($"Phòng yêu cầu: {roomId}");
            Console.WriteLine($"Thiết bị được chọn: {selected.Id} - {selected.Name}");
        });

        // 7. Chọn micro không dây khả dụng đầu tiên
        TestEdge("7. Chọn micro không dây khả dụng đầu tiên", () =>
        {
            FirstAvailableStrategy<WirelessMicrophone> strategy = new FirstAvailableStrategy<WirelessMicrophone>();
            AllocationService<WirelessMicrophone> service = new AllocationService<WirelessMicrophone>(microphoneRepository, strategy);

            WirelessMicrophone selected = service.Allocate("A203", device => true);

            Console.WriteLine($"Thiết bị được chọn: {selected.Id} - {selected.Name}");
        });

        // 8. Thay đổi chiến lược lựa chọn trong thời gian chạy và quan sát kết quả
        TestEdge("8. Thay đổi chiến lược lựa chọn trong thời gian chạy và quan sát kết quả", () =>
        {
            string roomId = "A203";

            AllocationService<ProjectorRemote> service = new AllocationService<ProjectorRemote>(
                projectorRepository,
                new HighestPriorityStrategy<ProjectorRemote>(device => device.GetDefaultPriorityScore(roomId)));

            projector2.Return();

            Console.WriteLine("Chiến lược ban đầu: HighestPriorityStrategy");
            Console.WriteLine($"Điểm ưu tiên PR01: {projector1.GetDefaultPriorityScore(roomId)}");
            Console.WriteLine($"Điểm ưu tiên PR02: {projector2.GetDefaultPriorityScore(roomId)}");

            service.ChangeStrategy(new FirstAvailableStrategy<ProjectorRemote>());

            Console.WriteLine("Chiến lược sau khi thay đổi: FirstAvailableStrategy");

            ProjectorRemote selected = service.Allocate(roomId, device => true);

            Console.WriteLine($"Thiết bị được chọn: {selected.Id} - {selected.Name}");
        });

        // 9. Thực hiện cấp phát khi không có thiết bị phù hợp
        TestEdge("9. Thực hiện cấp phát khi không có thiết bị phù hợp", () =>
        {
            AllocationService<ProjectorRemote> service = new AllocationService<ProjectorRemote>(
                projectorRepository,
                new FirstAvailableStrategy<ProjectorRemote>());

            service.Allocate("A203", device => false);
        });

        // 10. Kiểm tra thiết bị được chọn chuyển sang trạng thái Borrowed
        TestEdge("10. Kiểm tra thiết bị được chọn chuyển sang trạng thái Borrowed", () =>
        {
            Console.WriteLine($"Trạng thái PR01: {projector1.Status}");

            if (projector1.Status != DeviceStatus.Borrowed)
            {
                throw new Exception("PR01 không ở trạng thái Borrowed sau khi được cấp phát.");
            }

            Console.WriteLine("Kiểm tra thành công: PR01 đang ở trạng thái Borrowed.");
        });

        // 11. Kiểm tra repository generic hoạt động với ít nhất hai loại thiết bị mà không cần lặp lại mã nguồn
        TestEdge("11. Kiểm tra repository generic hoạt động với ít nhất hai loại thiết bị mà không cần lặp lại mã nguồn", () =>
        {
            IRepository<ProjectorRemote> projectorTestRepository = new InMemoryRepository<ProjectorRemote>();
            IRepository<WirelessMicrophone> microphoneTestRepository = new InMemoryRepository<WirelessMicrophone>();

            ProjectorRemote testProjector = new ProjectorRemote("TEST_PR01", "Máy chiếu kiểm thử", "Tủ kiểm thử", 80);
            WirelessMicrophone testMicrophone = new WirelessMicrophone("TEST_MI01", "Micro kiểm thử", "Tủ kiểm thử", 80, 75);

            projectorTestRepository.Add(testProjector);
            microphoneTestRepository.Add(testMicrophone);

            ProjectorRemote? foundProjector = projectorTestRepository.FindById("TEST_PR01");
            WirelessMicrophone? foundMicrophone = microphoneTestRepository.FindById("TEST_MI01");

            if (foundProjector == null || foundMicrophone == null)
            {
                throw new Exception("Repository không lưu hoặc tìm được thiết bị.");
            }

            Console.WriteLine($"Repository ProjectorRemote: {foundProjector.Id}");
            Console.WriteLine($"Repository WirelessMicrophone: {foundMicrophone.Id}");
        });
    }
}