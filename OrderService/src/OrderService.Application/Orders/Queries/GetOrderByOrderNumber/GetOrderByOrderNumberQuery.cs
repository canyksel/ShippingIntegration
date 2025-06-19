using MediatR;

namespace OrderService.Application.Orders.Queries.GetOrderByOrderNumber;

public class GetOrderByOrderNumberQuery : IRequest<GetOrderByOrderNumberDto>
{
    public string OrderNumber { get; set; }
}
