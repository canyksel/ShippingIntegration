using OrderService.Domain.Entities;
using OrderService.Domain.Repositories.CompanyAddress;
using OrderService.Infrastructure.Persistence;
using OrderService.Infrastructure.Repositories.Common;

namespace OrderService.Infrastructure.Repositories;

public class CompanyAddressRepository(OrderContext context) : EfRepository<CompanyAddress>(context), ICompanyAddressRepository
{
}