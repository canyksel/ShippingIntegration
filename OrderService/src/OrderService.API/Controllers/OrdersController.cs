using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Orders.Commands.CreateOrder;
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
        var result = await mediator.Send(new GetOrderByOrderNumberQuery { OrderNumber = orderNumber});
        return result != null ? Ok(result) : NotFound();
    }
}
