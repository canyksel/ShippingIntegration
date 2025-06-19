using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace OrderService.Infrastructure.Messaging;

public static class MassTransitExtensions
{
    public static IServiceCollection AddOrderServiceMessaging(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
            });
        });

        return services;
    }
}