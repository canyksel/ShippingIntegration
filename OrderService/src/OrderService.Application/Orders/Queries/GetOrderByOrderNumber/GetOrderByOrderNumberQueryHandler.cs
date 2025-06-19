using AutoMapper;
using MediatR;
using OrderService.Domain.Repositories.Order;

namespace OrderService.Application.Orders.Queries.GetOrderByOrderNumber;

public class GetOrderByOrderNumberQueryHandler(IOrderRepository orderRepository, IMapper mapper) : IRequestHandler<GetOrderByOrderNumberQuery, GetOrderByOrderNumberDto>
{
    public async Task<GetOrderByOrderNumberDto> Handle(GetOrderByOrderNumberQuery request, CancellationToken cancellationToken)
    {
        var order = await orderRepository
          .GetByOrderNumberWithDetailsAsync(request.OrderNumber) ?? throw new InvalidOperationException($"Orner not found: {request.OrderNumber}");

        return mapper.Map<GetOrderByOrderNumberDto>(order);
    }
}