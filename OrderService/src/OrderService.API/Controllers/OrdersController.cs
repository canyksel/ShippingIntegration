using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Orders.Commands.CancelOrder;
using OrderService.Application.Orders.Commands.CreateOrder;
using OrderService.Application.Orders.Commands.PaidOrder;
using OrderService.Application.Orders.Queries.GetOrderByOrderNumber;

namespace OrderService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command, CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetOrderByOrderNumber), new { orderNumber = result.OrderNumber }, result);
    }

    [HttpGet("{orderNumber}")]
    public async Task<IActionResult> GetOrderByOrderNumber([FromRoute] string orderNumber, CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new GetOrderByOrderNumberQuery { OrderNumber = orderNumber });
        return result != null ? Ok(result) : NotFound();
    }

    [HttpPost("{orderNumber}/paid")]
    public async Task<IActionResult> PaidOrder([FromRoute] string orderNumber, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new PaidOrderCommand { OrderNumber = orderNumber }, cancellationToken);
        return result ? Ok() : BadRequest();
    }

    [HttpPost("{orderNumber}/cancel")]
    public async Task<IActionResult> CancelOrder([FromRoute] string orderNumber, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CancelOrderCommand { OrderNumber = orderNumber }, cancellationToken);
        return result ? Ok() : BadRequest();
    }
}
