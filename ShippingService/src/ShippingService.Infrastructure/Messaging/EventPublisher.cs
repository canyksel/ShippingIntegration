using MassTransit;
using ShippingService.Application.Contracts.Events;

namespace ShippingService.Infrastructure.Messaging;

public class EventPublisher(IPublishEndpoint publishEndpoint) : IEventPublisher
{
    public async Task PublishShipmentStatusChangedAsync(ShipmentStatusChangedEvent @event)
    {
        await publishEndpoint.Publish(@event);
    }
}