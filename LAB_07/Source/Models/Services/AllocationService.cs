/****************/
// 202419017
// Đinh Thế Vinh
/****************/
using System;
using System.Collections.Generic;
using Source.Interfaces;
using Source.Models.Devices;

namespace Source.Services;

public class AllocationService<T>
    where T : TeachingDevice
{
    // Kho lưu trữ thiết bị, dùng để truy xuất danh sách thiết bị.
    private readonly IRepository<T> _repository;

    // Chiến lược lựa chọn thiết bị hiện tại.
    private ISelectionStrategy<T> _strategy;

    public AllocationService(IRepository<T> repository,ISelectionStrategy<T> strategy)
    {
        // 1. Kiểm tra kho lưu trữ hợp lệ.
        if (repository is null)
        {
            throw new ArgumentNullException(nameof(repository));
        }

        // 2. Kiểm tra chiến lược lựa chọn hợp lệ
        if (strategy is null)
        {
            throw new ArgumentNullException(nameof(strategy));
        }

        // 3. Khởi tạo các đối tượng phụ thuộc
        _repository = repository;
        _strategy = strategy;
    }

    public void ChangeStrategy(ISelectionStrategy<T> newStrategy)
    {
        // 1. Kiểm tra chiến lược mới hợp lệ.
        if (newStrategy is null)
        {
            throw new ArgumentNullException(nameof(newStrategy));
        }

        // 2. Thay thế chiến lược lựa chọn hiện tại
        _strategy = newStrategy;
    }

    public T Allocate(string roomId,Predicate<T>? additionalCondition = null)
    {
        // 1. Kiểm tra mã phòng hợp lệ.
        if (string.IsNullOrWhiteSpace(roomId))
        {
            throw new ArgumentException("Room ID cannot be null or empty.",nameof(roomId));
        }

        // 2. Lấy toàn bộ thiết bị từ kho lưu trữ
        IReadOnlyList<T> devices = _repository.GetAll();

        // 3. Lọc các thiết bị sẵn sàng cấp phát
        List<T> candidates = new List<T>();

        foreach (T device in devices)
        {
            if (device.IsReadyForUse())
            {
                candidates.Add(device);
            }
        }

        // 4. Áp dụng điều kiện bổ sung nếu được cung cấp.
        // Ví dụ: chỉ chọn điều khiển điều hòa tương thích với phòng
        if (additionalCondition is not null)
        {
            List<T> filteredCandidates = new List<T>();

            foreach (T device in candidates)
            {
                if (additionalCondition(device))
                {
                    filteredCandidates.Add(device);
                }
            }

            candidates = filteredCandidates;
        }

        // 5. Sử dụng chiến lược hiện tại để chọn thiết bị
        // Nếu không có ứng viên phù hợp, chiến lược cần phát sinh
        // NoSuitableItemException
        T selectedDevice = _strategy.Select(candidates);

        // 6. Cấp phát thiết bị cho phòng được yêu cầu
        selectedDevice.Borrow(roomId);

        // 7. Trả về thiết bị đã cấp phát
        return selectedDevice;
    }
}
