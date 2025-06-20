using ShippingService.Domain.Entities;
using ShippingService.Domain.Enums;

namespace ShippingService.Domain.Test;

public class ShipmentTest
{
    [Fact]
    public void ShouldCreateShipmentSuccessfully()
    {
        var orderId = Guid.NewGuid();
        var orderNumber = "ORD-123456";
        var shippingCompanyId = Guid.NewGuid();

        var shipment = new Shipment(orderId, orderNumber, shippingCompanyId);

        Assert.Equal(orderId, shipment.OrderId);
        Assert.Equal(orderNumber, shipment.OrderNumber);
        Assert.Equal(shippingCompanyId, shipment.ShippingCompanyId);
        Assert.Equal(ShipmentStatus.Prepared, shipment.Status);
        Assert.Null(shipment.LastUpdatedAt);
        Assert.True(shipment.CreatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void ShouldMoveShipmentToInTransit()
    {
        var shipment = new Shipment(Guid.NewGuid(), "ORD-TEST", Guid.NewGuid());

        shipment.MoveToInTransit();

        Assert.Equal(ShipmentStatus.InTransit, shipment.Status);
        Assert.NotNull(shipment.LastUpdatedAt);
    }

    [Fact]
    public void ShouldThrowWhenMovingToInTransitIfNotPrepared()
    {
        var shipment = new Shipment(Guid.NewGuid(), "ORD-TEST", Guid.NewGuid());
        shipment.MoveToInTransit();

        var ex = Assert.Throws<InvalidOperationException>(() => shipment.MoveToInTransit());
        Assert.Equal("Only Prepared shipments can move to InTransit.", ex.Message);
    }

    [Fact]
    public void ShouldMoveShipmentToDelivered()
    {
        var shipment = new Shipment(Guid.NewGuid(), "ORD-TEST", Guid.NewGuid());
        shipment.MoveToInTransit();

        shipment.MoveToDelivered();

        Assert.Equal(ShipmentStatus.Delivered, shipment.Status);
        Assert.NotNull(shipment.LastUpdatedAt);
    }

    [Fact]
    public void ShouldThrowWhenMovingToDeliveredIfNotInTransit()
    {
        var shipment = new Shipment(Guid.NewGuid(), "ORD-TEST", Guid.NewGuid());

        var ex = Assert.Throws<InvalidOperationException>(() => shipment.MoveToDelivered());
        Assert.Equal("Only InTransit shipments can move to Delivered.", ex.Message);
    }
}