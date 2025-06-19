using MediatR;
using ShippingService.Domain.Enums;

namespace ShippingService.Application.Shipments.Commands;

public class UpdateShipmentStatusCommand : IRequest<UpdateShipmentStatusResultDto>
{
    public string OrderNumber { get; set; }
    public ShipmentStatus NewStatus { get; set; }
}
