/****************/
// 202419017
// Đinh Thế Vinh
/****************/

using System;
using Source.Interfaces;
using Source.Models;

namespace Source.Patterns.Observers;

public sealed class EmergencyBridgeObserver : IAlertObserver
{
    private readonly IEmergencyMediator _mediator;

    public EmergencyBridgeObserver(IEmergencyMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public void Update(AlertEvent alertEvent)
    {
        ArgumentNullException.ThrowIfNull(alertEvent);

        if (alertEvent.Severity != "CRITICAL")
            return;

        // Chuyển cảnh báo khẩn cấp thành sự kiện điều phối thiết bị
        _mediator.Notify("FIRE_DETECTED");
    }
}
