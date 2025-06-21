using MassTransit;
using SharedKernel.Contracts.Events;
using ShippingService.Application.Events;

namespace ShippingService.Infrastructure.Messaging;

public class EventPublisher(IPublishEndpoint publishEndpoint) : IEventPublisher
{
    public async Task PublishShipmentStatusChangedAsync(ShipmentStatusChangedEvent @event)
    {
        await publishEndpoint.Publish(@event);
    }
}