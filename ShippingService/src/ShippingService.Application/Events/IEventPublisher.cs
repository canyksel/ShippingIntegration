using SharedKernel.Contracts.Events;

namespace ShippingService.Application.Events;

public interface IEventPublisher
{
    Task PublishShipmentStatusChangedAsync(ShipmentStatusChangedEvent @event);
}