namespace ShippingService.Application.Common;

public interface IShipmentCacheService
{
    Task SetShipmentStatusAsync(string orderNumber, string status);
    Task<string?> GetShipmentStatusAsync(string orderNumber);
}