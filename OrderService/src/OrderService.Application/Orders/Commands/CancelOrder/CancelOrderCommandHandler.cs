using MediatR;
using Microsoft.Extensions.Logging;
using OrderService.Domain.Enums;
using OrderService.Domain.Repositories.Order;

namespace OrderService.Application.Orders.Commands.CancelOrder;

public class CancelOrderCommandHandler(
    IOrderRepository orderRepository,
    ILogger<CancelOrderCommandHandler> logger)
    : IRequestHandler<CancelOrderCommand, bool>
{
    public async Task<bool> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByOrderNumberWithDetailsAsync(request.OrderNumber);
        if (order == null)
        {
            logger.LogWarning("Order not found for cancellation. OrderNumber: {OrderNumber}", request.OrderNumber);
            throw new InvalidOperationException("Order not found.");
        }

        if (order.Status is OrderStatus.Shipped or OrderStatus.Delivered)
        {
            logger.LogWarning("Cannot cancel an order that is already shipped or delivered. OrderNumber: {OrderNumber}", request.OrderNumber);
            throw new InvalidOperationException("Order cannot be cancelled after shipment.");
        }

        order.UpdateOrderStatus(OrderStatus.Cancelled);
        orderRepository.Update(order);
        var result = await orderRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Order cancelled successfully. OrderNumber: {OrderNumber}", request.OrderNumber);
        return result > 0;
    }
}