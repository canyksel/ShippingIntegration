namespace OrderService.Application.Orders.Events;

public interface IEventPublisher
{
    Task PublishOrderCreatedAsync(OrderCreatedEvent @event);
}
