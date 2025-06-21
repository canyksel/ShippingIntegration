using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Contracts.Events;
using ShippingService.Application.Common;
using ShippingService.Application.Events;
using ShippingService.Domain.Enums;
using ShippingService.Domain.Repositories.Shipment;

namespace ShippingService.Application.Shipments.Commands;

public class UpdateShipmentStatusCommandHandler(
    IShipmentCacheService shipmentCacheService,
    IEventPublisher eventPublisher,
    IShipmentRepository shipmentRepository,
    ILogger<UpdateShipmentStatusCommandHandler> logger)
    : IRequestHandler<UpdateShipmentStatusCommand, UpdateShipmentStatusResultDto>
{
    public async Task<UpdateShipmentStatusResultDto> Handle(UpdateShipmentStatusCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("UpdateShipmentStatusCommand received for OrderNumber: {OrderNumber} with TargetStatus: {TargetStatus}",
            request.OrderNumber, request.NewStatus);

        var shipment = await shipmentRepository.GetByOrderNumberAsync(request.OrderNumber);
        if (shipment is null)
        {
            logger.LogWarning("Shipment not found for OrderNumber: {OrderNumber}", request.OrderNumber);
            throw new InvalidOperationException("Shipment not found.");
        }

        switch (request.NewStatus)
        {
            case ShipmentStatus.Prepared:
                throw new InvalidOperationException("Shipment is already prepared.");
            case ShipmentStatus.InTransit:
                shipment.MoveToInTransit();
                await shipmentCacheService.SetShipmentStatusAsync(request.OrderNumber, request.NewStatus.ToString());
                break;
            case ShipmentStatus.Delivered:
                shipment.MoveToDelivered();
                await shipmentCacheService.SetShipmentStatusAsync(request.OrderNumber, request.NewStatus.ToString());
                break;
            default:
                throw new InvalidOperationException("Invalid status.");
        }

        shipmentRepository.Update(shipment);
        await shipmentRepository.UnitOfWork.SaveEntitesAsync(cancellationToken);

        if (request.NewStatus == ShipmentStatus.Delivered)
        {
            await eventPublisher.PublishShipmentStatusChangedAsync(new ShipmentStatusChangedEvent
            {
                OrderNumber = shipment.OrderNumber,
                NewStatus = "Delivered"
            });

            logger.LogInformation("Shipment Delivered Event published to OrderService");
        }

        return new UpdateShipmentStatusResultDto
        {
            ShipmentId = shipment.Id,
            OrderId = shipment.OrderId,
            OrderNumber = shipment.OrderNumber,
            UpdatedStatus = shipment.Status.ToString()
        };
    }
}