/****************/
// 202419017
// Đinh Thế Vinh
/****************/
using System;
using Source.Models.Devices;
using Source.Exceptions;

namespace Source.Models.Borrowing;

public class BorrowItem
{
    private Device _device;
    private DateTime _borrowedDate;
    private int _fromPeriod;
    private int _toPeriod;
    private DateTime _dueDate;
    private DateTime _returnedDate;
    private bool _isReturned;

    public Device Device
    {
        get => _device;
        private set => _device = value;
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

    public int FromPeriod
    {
        get => _fromPeriod;
        private set
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value), "From period must be positive.");

            _fromPeriod = value;
        }
    }

    public int ToPeriod
    {
        get => _toPeriod;
        private set
        {
            if (value < FromPeriod)
                throw new ArgumentOutOfRangeException(nameof(value), "To period cannot be earlier than from period.");

            _toPeriod = value;
        }
    }

    public DateTime DueDate
    {
        get => _dueDate;
        private set
        {
            if (value.Date < BorrowedDate.Date)
                throw new ArgumentException("Due date cannot be earlier than borrowed date.");

            _dueDate = value;
        }
    }

    public DateTime ReturnedDate
    {
        get => _returnedDate;
        private set
        {
            if (value != default && value.Date < BorrowedDate.Date)
            {
                throw new ArgumentException("Returned date cannot be earlier than borrowed date.");
            }

            _returnedDate = value;
        }
    }

    public bool IsReturned
    {
        get => _isReturned;
        private set => _isReturned = value;
    }

    public BorrowItem(Device device, DateTime borrowedDate, int fromPeriod, int toPeriod, DateTime dueDate)
    {
        Device = device;
        BorrowedDate = borrowedDate;
        FromPeriod = fromPeriod;
        ToPeriod = toPeriod;
        DueDate = dueDate;
        ReturnedDate = default;
        IsReturned = false;
    }

    public void Return(DateTime returnDate)
    {
        if (IsReturned)
            throw new InvalidOperationException("This device has already been returned.");

        ReturnedDate = returnDate;
        IsReturned = true;
    }
}