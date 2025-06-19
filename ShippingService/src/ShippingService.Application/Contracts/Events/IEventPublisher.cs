namespace ShippingService.Application.Contracts.Events;

public interface IEventPublisher
{
    Task PublishShipmentStatusChangedAsync(ShipmentStatusChangedEvent @event);
}