using MediatR;

namespace OrderService.Application.Orders.Commands.CancelOrder;

public class CancelOrderCommand : IRequest<bool>
{
    public string OrderNumber { get; set; }
}