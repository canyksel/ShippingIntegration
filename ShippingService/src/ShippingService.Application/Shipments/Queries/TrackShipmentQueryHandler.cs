using MediatR;
using Microsoft.Extensions.Logging;
using ShippingService.Application.Common;

namespace ShippingService.Application.Shipments.Queries;

public class TrackShipmentQueryHandler(
    IShipmentCacheService cacheService,
    ILogger<TrackShipmentQueryHandler> logger)
    : IRequestHandler<TrackShipmentQuery, string?>
{
    public async Task<string?> Handle(TrackShipmentQuery request, CancellationToken cancellationToken)
    {
        var status = await cacheService.GetShipmentStatusAsync(request.OrderNumber);

        logger.LogInformation("Track requested for {OrderNumber}. Status: {Status}", request.OrderNumber, status);

        return status;
    }
}