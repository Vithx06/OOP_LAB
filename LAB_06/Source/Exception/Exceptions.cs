/****************/
// 202419017
// Đinh Thế Vinh
/****************/
using System;
using Source.Enum;

namespace Source.Exceptions;
// Lớp ngoại lệ cơ sở 
public class BorrowingException : Exception
{
    public BorrowingException(string message) : base(message)
    {
    }

    public BorrowingException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

// Các lớp ngoại lệ dẫn xuất
// Ngoại lệ thiết bị không có sẵn
public class DeviceUnavailableException : BorrowingException
{
    public string DeviceId { get; }
    public DeviceStatus CurrentStatus { get; }

    public DeviceUnavailableException(string deviceId, DeviceStatus currentStatus)
    : base($"Device '{deviceId}' is unavailable. " + $"Current status: {currentStatus}.")
    {
        DeviceId = deviceId;
        CurrentStatus = currentStatus;
    }
}

// Ngoại lệ vượt hạn mức vay
public class BorrowLimitExceededException : BorrowingException
{
    public string BorrowerId { get; }
    public int MaximumDevices { get; }

    public BorrowLimitExceededException(string borrowerId, int maximumDevices)
    : base($"Borrower '{borrowerId}' has exceeded the borrowing limit. " + $"Maximum devices: {maximumDevices}.")
    {
        BorrowerId = borrowerId;
        MaximumDevices = maximumDevices;
    }
}

// Ngoại lệ thời gian mượn không hợp lệ
public class InvalidBorrowPeriodException : BorrowingException
{
    public string DeviceId { get; }
    public int RequestedDays { get; }
    public int MaxBorrowDays { get; }

    public InvalidBorrowPeriodException(string deviceId, int requestedDays, int maxDays)
    : base($"Invalid borrow period for device '{deviceId}'. " + $"Requested days: {requestedDays}, " + $"maximum borrow days: {maxDays}.")
    {
        DeviceId = deviceId;
        RequestedDays = requestedDays;
        MaxBorrowDays = maxDays;
    }
}

// Ngoại lệ trùng lặp thiết bị
public class DuplicateDeviceException : BorrowingException
{
    public string DeviceId { get; }

    public DuplicateDeviceException(string deviceId) : base($"Device '{deviceId}' already exists in the borrowing slip.")
    {
        DeviceId = deviceId;
    }
}

// Ngoại lệ thiết bị không có trong phiếu mượn
public class DeviceNotInSlipException : BorrowingException
{
    public string DeviceId { get; }

    public DeviceNotInSlipException(string deviceId) : base($"Device '{deviceId}' does not exist in the borrowing slip.")
    {
        DeviceId = deviceId;
    }
}

// Ngoại lệ thiết bị đã trả
public class DeviceAlreadyReturnedException : BorrowingException
{
    public string DeviceId { get; }

    public DeviceAlreadyReturnedException(string deviceId) : base($"Device '{deviceId}' has already been returned.")
    {
        DeviceId = deviceId;
    }
}

// Ngoại lệ ngày trả không hợp lệ
public class InvalidReturnDateException : BorrowingException
{
    public DateTime BorrowedDate { get; }
    public DateTime ReturnedDate { get; }

    public InvalidReturnDateException(DateTime borrowedDate, DateTime returnedDate)
    : base($"Invalid return date. " + $"Borrowed date: {borrowedDate}, " + $"returned date: {returnedDate}.")
    {
        BorrowedDate = borrowedDate;
        ReturnedDate = returnedDate;
    }
}

