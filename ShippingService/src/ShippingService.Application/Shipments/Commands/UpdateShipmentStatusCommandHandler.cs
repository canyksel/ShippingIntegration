using MediatR;
using Microsoft.Extensions.Logging;
using ShippingService.Domain.Enums;
using ShippingService.Domain.Repositories.Shipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Application.Shipments.Commands;

public class UpdateShipmentStatusCommandHandler(
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
                break;
            case ShipmentStatus.Delivered:
                shipment.MoveToDelivered();
                break;
            default:
                throw new InvalidOperationException("Invalid status.");
        }

        shipmentRepository.Update(shipment);
        await shipmentRepository.UnitOfWork.SaveEntitesAsync(cancellationToken);

        return new UpdateShipmentStatusResultDto
        {
            ShipmentId = shipment.Id,
            OrderId = shipment.OrderId,
            OrderNumber = shipment.OrderNumber,
            UpdatedStatus = shipment.Status.ToString()
        };
    }
}