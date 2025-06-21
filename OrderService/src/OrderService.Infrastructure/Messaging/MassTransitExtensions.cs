using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Consumers;

namespace OrderService.Infrastructure.Messaging;

public static class MassTransitExtensions
{
    public static IServiceCollection AddOrderServiceMessaging(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<ShipmentStatusChangedEventConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rabbitmq", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint("shipment-status-changed-event-queue", e =>
                {
                    e.ConfigureConsumer<ShipmentStatusChangedEventConsumer>(context);
                });
            });
        });

        return services;
    }
}