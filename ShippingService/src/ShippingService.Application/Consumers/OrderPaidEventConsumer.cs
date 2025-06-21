using MassTransit;
using Microsoft.Extensions.Logging;
using SharedKernel.Contracts.Events;
using ShippingService.Application.Common;
using ShippingService.Application.Events;
using ShippingService.Domain.Entities;
using ShippingService.Domain.Repositories.Shipment;

namespace ShippingService.Application.Consumers;

public class OrderPaidEventConsumer(
    IShipmentCacheService shipmentCacheService,
    IEventPublisher eventPublisher,
    IShipmentRepository shipmentRepository,
    ILogger<OrderPaidEventConsumer> logger)
    : IConsumer<OrderPaidEvent>
{
    public async Task Consume(ConsumeContext<OrderPaidEvent> context)
    {
        var message = context.Message;

        logger.LogInformation("[ShippingService] OrderPaidEvent received: OrderNumber={OrderNumber}, ShippingCompanyId={ShippingCompanyId}",
            message.OrderNumber, message.ShippingCompanyId);

        var shipment = new Shipment(
            orderId: message.OrderId,
            orderNumber: message.OrderNumber,
            shippingCompanyId: message.ShippingCompanyId
        );

        await shipmentRepository.AddAsync(shipment);
        var isCommitted = await shipmentRepository.UnitOfWork.SaveEntitesAsync(context.CancellationToken);
        if (isCommitted)
        {
            await eventPublisher.PublishShipmentStatusChangedAsync(new ShipmentStatusChangedEvent
            {
                OrderNumber = message.OrderNumber,
                NewStatus = "Prepared"
            });
            await shipmentCacheService.SetShipmentStatusAsync(message.OrderNumber, "Prepared");
            logger.LogInformation("[ShippingService] Shipment created with status Prepared for OrderNumber={OrderNumber}", message.OrderNumber);
        }
    }
}