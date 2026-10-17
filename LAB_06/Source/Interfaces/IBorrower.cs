/****************/
// 202419017
// Đinh Thế Vinh
/****************/
namespace Source.Interfaces;

public interface IBorrower
{
    string BorrowerId { get; }
    string FullName { get; }

    int GetMaximumDevices();
}
