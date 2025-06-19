using OrderService.Domain.Common.Interfaces;

namespace OrderService.Domain.Repositories.Order;

public interface IOrderRepository : IRepository<Domain.Entities.Order>
{
    Task<Domain.Entities.Order> GetByOrderNumberWithDetailsAsync(string orderNumber);
}