using MassTransit;
using OrderService.Application.Orders.Events;
using SharedKernel.Contracts.Events;

namespace OrderService.Infrastructure.Messaging;

public class EventPublisher(IPublishEndpoint publishEndpoint) : IEventPublisher
{
    public async Task PublishOrderPaidAsync(OrderPaidEvent @evente)
    {
        await publishEndpoint.Publish(@evente);
    }
}