using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Entities;
using OrderService.Domain.Repositories.Order;
using OrderService.Infrastructure.Persistence;
using OrderService.Infrastructure.Repositories.Common;

namespace OrderService.Infrastructure.Repositories;

public class OrderRepository(OrderContext context) : EfRepository<Order>(context), IOrderRepository
{
    public async Task<Order> GetByOrderNumberWithDetailsAsync(string orderNumber)
    {
        return await context.Orders.Include(o => o.Address)
                                .Include(o => o.ShippingCompany)
                                .Include(o => o.Products).ThenInclude(p => p.Product)
                                .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);
    }
}