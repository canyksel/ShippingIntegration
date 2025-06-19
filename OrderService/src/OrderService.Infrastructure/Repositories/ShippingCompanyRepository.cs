using OrderService.Domain.Entities;
using OrderService.Domain.Repositories.ShippingCompany;
using OrderService.Infrastructure.Persistence;
using OrderService.Infrastructure.Repositories.Common;

namespace OrderService.Infrastructure.Repositories;

public class ShippingCompanyRepository(OrderContext context) : EfRepository<ShippingCompany>(context), IShippingCompanyRepository
{
}