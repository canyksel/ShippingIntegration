using Microsoft.EntityFrameworkCore;
using ShippingService.Domain.Entities;
using ShippingService.Domain.Repositories.Shipment;
using ShippingService.Infrastructure.Persistence;
using ShippingService.Infrastructure.Repositories.Common;

namespace ShippingService.Infrastructure.Repositories;

public class ShipmentRepository(ShipmentContext context) : EfRepository<Shipment>(context), IShipmentRepository
{
    public async Task<Shipment?> GetByOrderIdAsync(Guid orderId)
    {
        return await context.Shipments
           .FirstOrDefaultAsync(s => s.OrderId == orderId);
    }

    public async Task<Shipment> GetByOrderNumberAsync(string orderNumber)
    {
        return await context.Shipments
            .FirstOrDefaultAsync(s => s.OrderNumber == orderNumber);
    }
}