using MediatR;

namespace ShippingService.Application.Shipments.Queries;
public class TrackShipmentQuery : IRequest<string>
{
    public string OrderNumber { get; set; }
}
