using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Domain.Common.Interfaces;
using OrderService.Domain.Repositories.CompanyAddress;
using OrderService.Domain.Repositories.Order;
using OrderService.Domain.Repositories.OrderAddress;
using OrderService.Domain.Repositories.OrderProduct;
using OrderService.Domain.Repositories.Product;
using OrderService.Domain.Repositories.ShippingCompany;
using OrderService.Infrastructure.Eventing;
using OrderService.Infrastructure.Repositories;
using OrderService.Infrastructure.Repositories.Common;
using System.Reflection;


namespace OrderService.Application.Extensions;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<IDomainEvent>());
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();


        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderProductRepository, OrderProductRepository>();
        services.AddScoped<IOrderAddressRepository, OrderAddressRepository>();
        services.AddScoped<ICompanyAddressRepository, CompanyAddressRepository>();
        services.AddScoped<IShippingCompanyRepository, ShippingCompanyRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped(typeof(IRepository<>), (typeof(EfRepository<>)));

        return services;
    }
}