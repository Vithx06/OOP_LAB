/****************/
// 202419017
// Đinh Thế Vinh
/****************/

using Source.Models;

namespace Source.Interfaces;

public interface IAlertObserver
{
    void Update(AlertEvent alertEvent);
}
