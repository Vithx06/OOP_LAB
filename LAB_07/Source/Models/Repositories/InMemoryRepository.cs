/****************/
// 202419017
// Đinh Thế Vinh
/****************/

using System;
using System.Collections.Generic;
using Source.Interfaces;

namespace Source.Models.Repositories;

public class InMemoryRepository<T> : IRepository<T>
    where T : IIdentifiable
{
    // Dùng Dict lưu với key: mã, value: thiết bị tương ứng 
    private readonly Dictionary<string, T> _items = new();

    public void Add(T item)
    {
        // 1. Kiểm tra thiết bị hợp lệ, không chấp nhận null
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        // 2. Kiểm tra mã thiết bị hợp lệ
        if (string.IsNullOrWhiteSpace(item.Id))
        {
            throw new ArgumentException("Item ID cannot be null or empty.",nameof(item));
        }

        // 3. Kiểm tra mã thiết bị đã tồn tại trong kho chưa = ContainsKey() kiểm tra xem Dict đã có Id này hay chưa
        if (_items.ContainsKey(item.Id))
        {
            throw new ArgumentException(
                $"An item with ID '{item.Id}' already exists.",
                nameof(item));
        }

        _items.Add(item.Id, item);
    }

    public bool Remove(string id)
    {
        // 1. Kiểm tra mã thiết bị hợp lệ
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException(
                "Item ID cannot be null or empty.",
                nameof(id));
        }

        return _items.Remove(id);
    }

    public T? FindById(string id)
    {
        // 1. Kiểm tra mã thiết bị hợp lệ
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("Item ID cannot be null or empty.",nameof(id));
        }

        // 2. Tìm thiết bị theo mã Id
        // TryGetValue() vừa kiểm tra khóa tồn tại, vừa lấy giá trị tương ứng
        if (_items.TryGetValue(id, out T? item))
        {
            return item;
        }

        return default;
    }

    public IReadOnlyList<T> GetAll()
    {
        return new List<T>(_items.Values);
    }

    public IReadOnlyList<T> FindAll(Predicate<T> condition)
    {
        // 1. Kiểm tra điều kiện lọc có hợp lệ hay không
        if (condition is null)
        {
            throw new ArgumentNullException(nameof(condition));
        }

        // 2. Tạo danh sách lưu các thiết bị thỏa mãn điều kiện lọc
        List<T> result = new List<T>();

        // 3. Duyệt qua tất cả thiết bị trong kho
        foreach (T item in _items.Values)
        {
            if (condition(item))
            {
                result.Add(item);
            }
        }

        return result;
    }
}
