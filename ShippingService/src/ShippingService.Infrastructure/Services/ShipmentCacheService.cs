using Microsoft.EntityFrameworkCore.Storage;
using ShippingService.Application.Common;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ShipmentCacheService(IConnectionMultiplexer redis) : IShipmentCacheService
{
    private readonly StackExchange.Redis.IDatabase _db = redis.GetDatabase();

    public async Task SetShipmentStatusAsync(string orderNumber, string status)
    {
        var key = $"shipment-status:{orderNumber}";
        await _db.StringSetAsync(key, status, TimeSpan.FromDays(1));
    }

    public async Task<string?> GetShipmentStatusAsync(string orderNumber)
    {
        var key = $"shipment-status:{orderNumber}";
        return await _db.StringGetAsync(key);
    }
}