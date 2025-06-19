using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Orders.Events;
using OrderService.Domain.Common.Interfaces;
using OrderService.Domain.Repositories.CompanyAddress;
using OrderService.Domain.Repositories.Order;
using OrderService.Domain.Repositories.OrderAddress;
using OrderService.Domain.Repositories.OrderProduct;
using OrderService.Domain.Repositories.Product;
using OrderService.Domain.Repositories.ShippingCompany;
using OrderService.Infrastructure.Eventing;
using OrderService.Infrastructure.Messaging;
using OrderService.Infrastructure.Repositories;
using OrderService.Infrastructure.Repositories.Common;

namespace OrderService.Infrastructure.Extensions;

public static class InfrastructureServicesExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderProductRepository, OrderProductRepository>();
        services.AddScoped<IOrderAddressRepository, OrderAddressRepository>();
        services.AddScoped<ICompanyAddressRepository, CompanyAddressRepository>();
        services.AddScoped<IShippingCompanyRepository, ShippingCompanyRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        services.AddScoped<IEventPublisher, EventPublisher>();
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

        return services;
    }
}
