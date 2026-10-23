/****************/
// 202419017
// Đinh Thế Vinh
/****************/
using System;
using System.Collections.Generic;
using System.Linq;
using Source.Interfaces;

namespace Source.Services.Strategies;

public class FirstAvailableStrategy<T> : ISelectionStrategy<T>
{
    public T Select(IReadOnlyList<T> candidates)
    {
        if (candidates == null)
        {
            throw new ArgumentNullException(nameof(candidates));
        }
        // Phương thức lấy phần tử đầu tiên trong tập các thiết bị
        T? selected = candidates.FirstOrDefault();

        if (selected == null)
        {
            throw new InvalidOperationException("No suitable device is available.");
        }

        return selected;
    }
}