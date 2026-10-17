/****************/
// 202419017
// Đinh Thế Vinh
/****************/
using System;
using Source.Interfaces;

namespace Source.Models.People;

public class Lecturer : IBorrower
{
    private string _borrowerId;
    private string _fullName;

    public string BorrowerId
    {
        get => _borrowerId;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Borrower ID cannot be empty.", nameof(value));

            _borrowerId = value;
        }
    }

    public string FullName
    {
        get => _fullName;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Full name cannot be empty.", nameof(value));

            _fullName = value;
        }
    }

    public Lecturer(string borrowerId, string fullName)
    {
        BorrowerId = borrowerId;
        FullName = fullName;
    }

    public int GetMaximumDevices()
    {
        return 3;
    }
}
