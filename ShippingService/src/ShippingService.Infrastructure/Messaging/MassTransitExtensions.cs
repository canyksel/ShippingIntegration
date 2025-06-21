using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using ShippingService.Application.Consumers;

namespace ShippingService.Infrastructure.Messaging;

public static class MassTransitExtensions
{
    public static IServiceCollection AddShipmentServiceMessaging(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<OrderPaidEventConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint("order-paid-event-queue", e =>
                {
                    e.ConfigureConsumer<OrderPaidEventConsumer>(context);
                });
            });
        });

        return services;
    }
}