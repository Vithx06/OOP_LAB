/****************/
// 202419017
// Đinh Thế Vinh
/****************/
public interface INetworkable
{
    string? IpAddress { get; }

    bool IsConnected { get; }

    void Connect(string ipAddress);

    void Disconnect();
}