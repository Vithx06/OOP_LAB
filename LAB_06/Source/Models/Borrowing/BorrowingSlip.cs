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

namespace Source.Models.Borrowing;

public class BorrowingSlip
{
    private string _slipId;
    private IBorrower _borrower;
    private string _teachingRoom;
    private DateTime _borrowedDate;
    private List<BorrowItem> _items = new();

    public string SlipId
    {
        get => _slipId;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Slip ID cannot be empty.");

            _slipId = value;
        }
    }

    public IBorrower Borrower
    {
        get => _borrower;
        private set => _borrower = value;
    }

    public string TeachingRoom
    {
        get => _teachingRoom;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Teaching room cannot be empty.");

            _teachingRoom = value;
        }
    }

    public DateTime BorrowedDate
    {
        get => _borrowedDate;
        private set
        {
            if (value == default)
                throw new ArgumentException("Borrowed date is invalid.");

            _borrowedDate = value;
        }
    }

    public List<BorrowItem> Items
    {
        get => _items;
        private set => _items = value;
    }

    public BorrowingSlip(string slipId, IBorrower borrower, string teachingRoom, DateTime borrowedDate)
    {
        SlipId = slipId;
        Borrower = borrower;
        TeachingRoom = teachingRoom;
        BorrowedDate = borrowedDate;
    }

    public void AddDevice(Device device, int borrowDays)
    {
        // 1. Kiểm tra thiết bị đã có trong phiếu chưa.
        foreach (BorrowItem Bi in Items)
        {
            if (Bi.Device.DeviceId == device.DeviceId)
            {
                throw new DuplicateDeviceException(device.DeviceId);
            }
        }

        // 2. Kiểm tra giới hạn số thiết bị được mượn.
        int maximumDevices = Borrower.GetMaximumDevices();

        if (Items.Count >= maximumDevices)
        {
            throw new BorrowLimitExceededException(Borrower.BorrowerId, maximumDevices);
        }

        // 3. Kiểm tra trạng thái thiết bị.
        if (device.Status != DeviceStatus.Available)
        {
            throw new DeviceUnavailableException(device.DeviceId, device.Status);
        }

        // 4. Kiểm tra số ngày mượn.
        if (borrowDays <= 0 || borrowDays > device.MaxBorrowDays)
        {
            throw new InvalidBorrowPeriodException(device.DeviceId, borrowDays, device.MaxBorrowDays);
        }

        // 5. Tính ngày đến hạn từ ngày mượn của phiếu.
        DateTime dueDate = BorrowedDate.AddDays(borrowDays);

        // 6. Tạo BorrowItem trước khi thay đổi trạng thái.
        BorrowItem item = new(device, BorrowedDate, 1, borrowDays, dueDate);

        // 7. Cập nhật trạng thái và danh sách.
        device.MarkAsBorrowed();
        Items.Add(item);
    }

    public decimal ReturnDevice(string deviceId, DateTime returnedDate)
    {
        // 1. Tìm thiết bị trong phiếu.
        BorrowItem? item = null;

        foreach (BorrowItem Bi in Items)
        {
            if (Bi.Device.DeviceId == deviceId)
            {
                item = Bi;
                break;
            }
        }

        if (item == null)
        {
            throw new DeviceNotInSlipException(deviceId);
        }

        // 2. Kiểm tra thiết bị đã được trả chưa.
        if (item.IsReturned)
        {
            throw new DeviceAlreadyReturnedException(deviceId);
        }

        // 3. Ghi nhận ngày trả.
        item.Return(returnedDate);

        // 4. Cập nhật trạng thái thiết bị.
        item.Device.MarkAsAvailable();

        // 5. Tính phí trả muộn của thiết bị này.
        int lateDays = (returnedDate.Date - item.DueDate.Date).Days;

        if (lateDays <= 0)
        {
            return 0;
        }

        return item.Device.CalculateLateFee(lateDays);
    }

    public decimal CalculateTotalLateFee()
    {
        decimal totalLateFee = 0;

        foreach (BorrowItem Bi in Items)
        {
            // Chỉ tính phí cho thiết bị đã trả.
            if (Bi.IsReturned)
            {
                int lateDays = (Bi.ReturnedDate.Date - Bi.DueDate.Date).Days;

                if (lateDays > 0)
                {
                    totalLateFee += Bi.Device.CalculateLateFee(lateDays);
                }
            }
        }

        return totalLateFee;
    }
}
