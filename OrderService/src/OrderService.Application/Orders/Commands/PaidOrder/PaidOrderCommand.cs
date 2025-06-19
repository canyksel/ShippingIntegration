using MediatR;

namespace OrderService.Application.Orders.Commands.PaidOrder;

public class PaidOrderCommand : IRequest<bool>
{
    public string OrderNumber { get; set; }
}
