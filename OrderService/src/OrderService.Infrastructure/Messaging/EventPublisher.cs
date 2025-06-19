using MassTransit;
using OrderService.Application.Orders.Events;

namespace OrderService.Infrastructure.Messaging;

public class EventPublisher(IPublishEndpoint publishEndpoint) : IEventPublisher
{
    public async Task PublishOrderCreatedAsync(OrderCreatedEvent evt)
    {
        await publishEndpoint.Publish(evt);
    }
}
