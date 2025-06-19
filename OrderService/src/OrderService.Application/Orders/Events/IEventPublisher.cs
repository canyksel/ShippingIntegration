namespace OrderService.Application.Orders.Events;

public interface IEventPublisher
{
    Task PublishOrderPaidAsync(OrderPaidEvent @event);
}
