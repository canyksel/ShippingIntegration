using MediatR;

namespace OrderService.Application.Orders.Commands.PaidOrder;

public class PaidOrderCommand : IRequest<PaidOrderDto>
{
    public string OrderNumber { get; set; }
}
