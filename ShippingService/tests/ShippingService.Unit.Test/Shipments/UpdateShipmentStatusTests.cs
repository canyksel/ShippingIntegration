using Microsoft.Extensions.Logging;
using Moq;
using ShippingService.Application.Common;
using ShippingService.Application.Contracts.Events;
using ShippingService.Application.Shipments.Commands;
using ShippingService.Domain.Entities;
using ShippingService.Domain.Enums;
using ShippingService.Domain.Repositories.Shipment;

namespace ShippingService.Unit.Test.Shipments;

public class UpdateShipmentStatusTests
{
    [Fact]
    public async Task HappyPath()
    {
        var shipment = new Shipment(Guid.NewGuid(), "ORD123", Guid.NewGuid());

        var mockShipmentRepository = new Mock<IShipmentRepository>();
        mockShipmentRepository.Setup(x => x.GetByOrderNumberAsync(It.IsAny<string>())).ReturnsAsync(shipment);

        var mockCacheService = new Mock<IShipmentCacheService>();
        var mockPublisher = new Mock<IEventPublisher>();
        var mockLogger = new Mock<ILogger<UpdateShipmentStatusCommandHandler>>();

        mockShipmentRepository.Setup(r => r.UnitOfWork.SaveEntitesAsync(It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

        var handler = new UpdateShipmentStatusCommandHandler(
            mockCacheService.Object,
            mockPublisher.Object,
            mockShipmentRepository.Object,
            mockLogger.Object
        );

        var command = new UpdateShipmentStatusCommand
        {
            OrderNumber = shipment.OrderNumber,
            NewStatus = ShipmentStatus.InTransit
        };

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(ShipmentStatus.InTransit.ToString(), result.UpdatedStatus);
        mockCacheService.Verify(x => x.SetShipmentStatusAsync(shipment.OrderNumber, "InTransit"), Times.Once);
        mockShipmentRepository.Verify(x => x.Update(shipment), Times.Once);
    }

    [Fact]
    public async Task ShouldThrowExceptionWhenShipmentNotFound()
    {
        var mockShipmentRepo = new Mock<IShipmentRepository>();
        mockShipmentRepo.Setup(x => x.GetByOrderNumberAsync(It.IsAny<string>())).ReturnsAsync((Shipment)null);

        var mockCacheService = new Mock<IShipmentCacheService>();
        var mockPublisher = new Mock<IEventPublisher>();
        var mockLogger = new Mock<ILogger<UpdateShipmentStatusCommandHandler>>();

        var handler = new UpdateShipmentStatusCommandHandler(
            mockCacheService.Object,
            mockPublisher.Object,
            mockShipmentRepo.Object,
            mockLogger.Object
        );

        var command = new UpdateShipmentStatusCommand
        {
            OrderNumber = "ORD999",
            NewStatus = ShipmentStatus.InTransit
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }
}