﻿using MediatR;
using Microsoft.Extensions.Logging;
using OrderService.Application.Extensions;
using OrderService.Application.Orders.Events;
using OrderService.Domain.Enums;
using OrderService.Domain.Repositories.Order;
using SharedKernel.Contracts.Events;

namespace OrderService.Application.Orders.Commands.PaidOrder;

public class PaidOrderCommandHandler(
    IOrderRepository orderRepository,
    IEventPublisher eventPublisher,
    ILogger<PaidOrderCommandHandler> logger)
    : IRequestHandler<PaidOrderCommand, PaidOrderDto>
{
    public async Task<PaidOrderDto> Handle(PaidOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByOrderNumberWithDetailsAsync(request.OrderNumber);
        if (order == null)
        {
            logger.LogWarning("Order not found for OrderNumber: {OrderNumber}", request.OrderNumber);
            throw new InvalidOperationException("Order not found.");
        }

        if (order.Status != OrderStatus.Pending)
        {
            logger.LogWarning("Order is not in Pending status. Current: {Status}", order.Status);
            throw new InvalidOperationException("Only Pending orders can be paid.");
        }

        order.UpdateOrderStatus(OrderStatus.Paid);
        orderRepository.Update(order);
        var isCommitted = await orderRepository.UnitOfWork.SaveEntitesAsync(cancellationToken);

        logger.LogInformation("Order marked as paid. Publishing event...");

        if (isCommitted)
        {
            await eventPublisher.PublishOrderPaidAsync(new OrderPaidEvent
            {
                OrderId = order.Id,
                OrderNumber = order.OrderNumber,
                ShippingCompanyId = order.ShippingCompanyId,
                Products = order.Products.Select(p => new OrderPaidEvent.ProductItem
                {
                    ProductId = p.ProductId,
                    Quantity = p.Quantity
                }).ToList()
            });
        }

        return new PaidOrderDto
        {
            OrderNumber = order.OrderNumber,
            OrderStatus = order.Status.GetDescription(),
            PaymentType = order.PaymentType.GetDescription(),
        };
    }
}