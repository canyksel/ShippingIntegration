using OrderService.Domain.Entities;
using OrderService.Domain.Repositories.OrderProduct;
using OrderService.Infrastructure.Persistence;
using OrderService.Infrastructure.Repositories.Common;

namespace OrderService.Infrastructure.Repositories;

public class OrderProductRepository(OrderContext context) : EfRepository<OrderProduct>(context), IOrderProductRepository
{
}