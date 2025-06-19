using ShippingService.Domain.Common.Interfaces;

namespace ShippingService.Domain.Repositories.Shipment;

public interface IShipmentRepository : IRepository<Entities.Shipment>
{
    Task<Entities.Shipment?> GetByOrderIdAsync(Guid orderId);
    Task<Entities.Shipment?> GetByOrderNumberAsync(string orderNumber);
}