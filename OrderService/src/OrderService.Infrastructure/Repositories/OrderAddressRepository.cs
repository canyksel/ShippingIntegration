using OrderService.Domain.Entities;
using OrderService.Domain.Repositories.OrderAddress;
using OrderService.Infrastructure.Persistence;
using OrderService.Infrastructure.Repositories.Common;

namespace OrderService.Infrastructure.Repositories;

public class OrderAddressRepository(OrderContext context) : EfRepository<OrderAddress>(context), IOrderAddressRepository
{
}