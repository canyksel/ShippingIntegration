using Microsoft.Extensions.DependencyInjection;
using ShippingService.Application.Common;
using ShippingService.Application.Events;
using ShippingService.Domain.Common.Interfaces;
using ShippingService.Domain.Repositories.Shipment;
using ShippingService.Infrastructure.Eventing;
using ShippingService.Infrastructure.Messaging;
using ShippingService.Infrastructure.Repositories;
using ShippingService.Infrastructure.Repositories.Common;

namespace ShippingService.Infrastructure.Extensions;

public static class InfrastructureServicesExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

        services.AddScoped<IShipmentRepository, ShipmentRepository>();
        services.AddScoped<IShipmentCacheService, ShipmentCacheService>();


        services.AddScoped<IEventPublisher, EventPublisher>();
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

        return services;
    }
}
