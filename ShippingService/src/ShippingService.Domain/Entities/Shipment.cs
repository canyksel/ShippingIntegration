using ShippingService.Domain.Common;
using ShippingService.Domain.Enums;

namespace ShippingService.Domain.Entities;

public class Shipment : EntityBase<Guid>
{
    public Guid OrderId { get; private set; }
    public string OrderNumber { get; private set; }
    public Guid ShippingCompanyId { get; private set; }

    public ShipmentStatus Status { get; private set; }
    public DateTime? LastUpdatedAt { get; private set; }

    protected Shipment() { }

    public Shipment(Guid orderId, string orderNumber, Guid shippingCompanyId)
    {
        Id = Guid.NewGuid();
        OrderId = orderId;
        OrderNumber = orderNumber;
        ShippingCompanyId = shippingCompanyId;
        Status = ShipmentStatus.Prepared;
        CreatedAt = DateTime.UtcNow;
    }

    public void MoveToInTransit()
    {
        if (Status != ShipmentStatus.Prepared)
            throw new InvalidOperationException("Only Prepared shipments can move to InTransit.");

        Status = ShipmentStatus.InTransit;
        LastUpdatedAt = DateTime.UtcNow;
    }

    public void MoveToDelivered()
    {
        if (Status != ShipmentStatus.InTransit)
            throw new InvalidOperationException("Only InTransit shipments can move to Delivered.");

        Status = ShipmentStatus.Delivered;
        LastUpdatedAt = DateTime.UtcNow;
    }
}
