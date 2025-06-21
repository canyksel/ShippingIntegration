using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace ShippingService.Infrastructure.Extensions;

public static class RedisServiceExtension
{
    public static IServiceCollection RegisterRedisService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var redisConnectionString = configuration.GetConnectionString("Redis");
            try
            {
                return ConnectionMultiplexer.Connect(redisConnectionString);
            }
            catch (RedisConnectionException ex)
            {
                var logger = sp.GetRequiredService<ILogger<ConnectionMultiplexer>>();
                logger.LogWarning("Redis'e bağlanılamadı: {Message}", ex.Message);
                return null!;
            }
        });

        return services;
    }
}
