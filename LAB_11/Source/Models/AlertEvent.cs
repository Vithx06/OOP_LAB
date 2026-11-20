/****************/
// 202419017
// Đinh Thế Vinh
/****************/

using System;

namespace Source.Models;

public sealed record AlertEvent(
    string SensorName,
    double Value,
    string Unit,
    string Severity,
    DateTime OccurredAt);
