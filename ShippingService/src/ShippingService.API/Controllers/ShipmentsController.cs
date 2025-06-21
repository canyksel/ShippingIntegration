using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShippingService.Application.Shipments.Commands;
using ShippingService.Application.Shipments.Queries;

namespace ShippingService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShipmentsController(IMediator mediator) : ControllerBase
{
    [HttpPost("update-order-status")]
    public async Task<IActionResult> UpdateShipmentStatus([FromBody] UpdateShipmentStatusCommand command, CancellationToken cancellationToken)
    {
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