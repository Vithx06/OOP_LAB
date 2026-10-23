/****************/
// 202419017
// Đinh Thế Vinh
/****************/
using System;
using System.Collections.Generic;
using Source.Interfaces;

namespace Source.Models.Strategies;

public class HighestPriorityStrategy<T> : ISelectionStrategy<T>
{
    // Hàm tổng quát tính điểm ~ 1 thuộc tính
    private readonly Func<T, double> _scoreSelector;

    // constructor
    public HighestPriorityStrategy(Func<T, double> scoreSelector)
    {
        if (scoreSelector == null)
        {
            throw new ArgumentNullException(nameof(scoreSelector));
        }

        _scoreSelector = scoreSelector;
    }

    public T Select(IReadOnlyList<T> candidates)
    {
        if (candidates == null)
        {
            throw new ArgumentNullException(nameof(candidates));
        }

        if (candidates.Count == 0)
        {
            throw new InvalidOperationException("No suitable device is available.");
        }

        // Logic lấy điểm ưu tiên cao nhất
        T selected = candidates[0];
        double highestScore = _scoreSelector(selected);

        for (int i = 1; i < candidates.Count; i++)
        {
            T candidate = candidates[i];
            double currentscore = _scoreSelector(candidate);

            if (currentscore > highestScore)
            {
                highestScore = currentscore;
                selected = candidate;
            }
        }

        return selected;
    }
}