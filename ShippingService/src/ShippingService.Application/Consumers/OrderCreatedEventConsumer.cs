using MassTransit;
using Microsoft.Extensions.Logging;
using ShippingService.Application.Contracts.Events;
using ShippingService.Domain.Entities;
using ShippingService.Domain.Repositories.Shipment;

namespace ShippingService.Application.Consumers;

public class OrderCreatedEventConsumer(IShipmentRepository shipmentRepository, ILogger<OrderCreatedEventConsumer> logger) : IConsumer
{
    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var message = context.Message;

        logger.LogInformation("OrderCreatedEvent received. OrderNumber: {OrderNumber}, ShippingCompanyId: {ShippingCompanyId}",
            message.OrderNumber, message.ShippingCompanyId);

        var shipment = new Shipment(message.OrderId, message.OrderNumber, message.ShippingCompanyId);

        await shipmentRepository.AddAsync(shipment);
        await shipmentRepository.UnitOfWork.SaveEntitesAsync(context.CancellationToken);
    }
}