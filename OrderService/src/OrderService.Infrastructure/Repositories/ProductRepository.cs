using OrderService.Domain.Entities;
using OrderService.Domain.Repositories.Product;
using OrderService.Infrastructure.Persistence;
using OrderService.Infrastructure.Repositories.Common;

namespace OrderService.Infrastructure.Repositories;

public class ProductRepository(OrderContext context) : EfRepository<Product>(context), IProductRepository
{
}