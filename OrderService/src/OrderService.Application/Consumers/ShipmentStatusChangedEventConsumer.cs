using MassTransit;
using Microsoft.Extensions.Logging;
using OrderService.Application.Contracts.Events;
using OrderService.Domain.Enums;
using OrderService.Domain.Repositories.Order;

namespace OrderService.Application.Consumers;

public class ShipmentStatusChangedEventConsumer(
    IOrderRepository orderRepository,
    ILogger<ShipmentStatusChangedEventConsumer> logger)
    : IConsumer<ShipmentStatusChangedEvent>
{
    public async Task Consume(ConsumeContext<ShipmentStatusChangedEvent> context)
    {
        var message = context.Message;

        logger.LogInformation("ShipmentStatusChangedEvent received: OrderNumber={OrderNumber}, NewStatus={NewStatus}",
            message.OrderNumber, message.NewStatus);

        var order = await orderRepository.FirstOrDefaultAsync(o => o.OrderNumber == message.OrderNumber);
        if (order == null)
        {
            logger.LogWarning("Order not found for OrderNumber: {OrderNumber}", message.OrderNumber);
            return;
        }

        if (message.NewStatus == "Shipped")
        {
            order.UpdateOrderStatus(OrderStatus.Shipped);
        }
        else if (message.NewStatus == "Delivered")
        {
            order.UpdateOrderStatus(OrderStatus.Delivered);
        }
        else
        {
            logger.LogWarning("Unknown shipment status received: {Status}", message.NewStatus);
            return;
        }

        orderRepository.Update(order);
        await orderRepository.UnitOfWork.SaveEntitesAsync(context.CancellationToken);

        logger.LogInformation("Order status updated to {Status} for OrderNumber={OrderNumber}", order.Status, message.OrderNumber);
    }
}