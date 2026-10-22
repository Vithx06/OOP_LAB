/****************/
// 202419017
// Đinh Thế Vinh
/****************/
namespace Source.Interfaces;

// Giao diện quy định các đối tượng phải có thuộc tính Id
public interface IIdentifiable
{
    string Id { get; }
}

// Giao diện quy định các thao tác cơ bản của 1 kho lưu trữ thiết bị
public interface IRepository<T> where T : IIdentifiable
{
    void Add(T item);
    bool Remove(string id);
    T? FindById(string id);
    IReadOnlyList<T> GetAll();
    IReadOnlyList<T> FindAll(Predicate<T> condition);
}

// Giao diện quy định chiến lược chọn thiết bị cho đối tượng
public interface ISelectionStrategy<T>
{
    T Select(IReadOnlyList<T> candidates);
}
