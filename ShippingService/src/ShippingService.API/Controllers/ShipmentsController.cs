using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShippingService.Application.Shipments.Commands;
using ShippingService.Application.Shipments.Queries;
using ShippingService.Domain.Enums;

namespace ShippingService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShipmentsController(IMediator mediator) : ControllerBase
{
    [HttpPost("{orderNumber}/update-status")]
    public async Task<IActionResult> UpdateShipmentStatus([FromRoute] string orderNumber, [FromBody] ShipmentStatus newStatus,CancellationToken cancellationToken)
    {
        var command = new UpdateShipmentStatusCommand
        {
            OrderNumber = orderNumber,
            NewStatus = newStatus
        };

        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpGet("track/{orderNumber}")]
    public async Task<IActionResult> TrackShipment([FromRoute] string orderNumber, CancellationToken cancellationToken)
    {
        var status = await mediator.Send(new TrackShipmentQuery { OrderNumber = orderNumber }, cancellationToken);

        if (status == null)
            return NotFound("No shipment found for this order.");

        return Ok(new { OrderNumber = orderNumber, Status = status });
    }
}