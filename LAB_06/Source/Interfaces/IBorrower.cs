/****************/
// 202419017
// Đinh Thế Vinh
/****************/

public interface IBorrower
{
    public string BorrowID {get; }
    public string FullName {get; }

    int GetMaximunDevices();
}