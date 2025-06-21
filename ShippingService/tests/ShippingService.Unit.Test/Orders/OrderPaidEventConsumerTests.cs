using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using SharedKernel.Contracts.Events;
using ShippingService.Application.Common;
using ShippingService.Application.Consumers;
using ShippingService.Application.Events;
using ShippingService.Domain.Entities;
using ShippingService.Domain.Repositories.Shipment;

namespace ShippingService.Unit.Test.Orders;

public class OrderPaidEventConsumerTests
{

    [Fact]
    public async Task ConsumeShouldCreateShipmentAndPublishEventAndCacheStatusWhenCommitted()
    {
        var mockCacheService = new Mock<IShipmentCacheService>();
        var mockPublisher = new Mock<IEventPublisher>();
        var mockShipmentRepository = new Mock<IShipmentRepository>();
        var mockLogger = new Mock<ILogger<OrderPaidEventConsumer>>();

        var consumer = new OrderPaidEventConsumer(
            mockCacheService.Object,
            mockPublisher.Object,
            mockShipmentRepository.Object,
            mockLogger.Object);

        var message = new OrderPaidEvent
        {
            OrderId = Guid.NewGuid(),
            OrderNumber = "ORD-123456",
            ShippingCompanyId = Guid.NewGuid(),
            Products = []
        };

        var mockContext = new Mock<ConsumeContext<OrderPaidEvent>>();
        mockContext.Setup(x => x.Message).Returns(message);
        mockContext.Setup(x => x.CancellationToken).Returns(CancellationToken.None);

        mockShipmentRepository.Setup(r => r.UnitOfWork.SaveEntitesAsync(It.IsAny<CancellationToken>()))
                      .ReturnsAsync(true);

        await consumer.Consume(mockContext.Object);

        mockShipmentRepository.Verify(r => r.AddAsync(It.Is<Shipment>(s =>
            s.OrderId == message.OrderId &&
            s.OrderNumber == message.OrderNumber &&
            s.ShippingCompanyId == message.ShippingCompanyId)), Times.Once);

        mockShipmentRepository.Verify(r => r.UnitOfWork.SaveEntitesAsync(It.IsAny<CancellationToken>()), Times.Once);
        mockPublisher.Verify(p => p.PublishShipmentStatusChangedAsync(It.Is<ShipmentStatusChangedEvent>(e =>
            e.OrderNumber == message.OrderNumber &&
            e.NewStatus == "Prepared")), Times.Once);

        mockCacheService.Verify(c => c.SetShipmentStatusAsync(message.OrderNumber, "Prepared"), Times.Once);

        mockLogger.Verify(l => l.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Shipment created")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }

    [Fact]
    public async Task ConsumeShouldNotPublishOrCacheWhenSaveEntitesFails()
    {
        var mockCacheService = new Mock<IShipmentCacheService>();
        var mockPublisher = new Mock<IEventPublisher>();
        var mockShipmentRepository = new Mock<IShipmentRepository>();
        var mockLogger = new Mock<ILogger<OrderPaidEventConsumer>>();

        var handler = new OrderPaidEventConsumer(
            mockCacheService.Object,
            mockPublisher.Object,
            mockShipmentRepository.Object,
            mockLogger.Object);

        var message = new OrderPaidEvent
        {
            OrderId = Guid.NewGuid(),
            OrderNumber = "ORD-123456",
            ShippingCompanyId = Guid.NewGuid(),
            Products = []
        };

        var mockContext = new Mock<ConsumeContext<OrderPaidEvent>>();
        mockContext.Setup(x => x.Message).Returns(message);
        mockContext.Setup(x => x.CancellationToken).Returns(CancellationToken.None);

        mockShipmentRepository.Setup(r => r.UnitOfWork.SaveEntitesAsync(It.IsAny<CancellationToken>()))
                      .ReturnsAsync(false);

        await handler.Consume(mockContext.Object);

        mockShipmentRepository.Verify(r => r.UnitOfWork.SaveEntitesAsync(It.IsAny<CancellationToken>()), Times.Once);
        mockPublisher.Verify(p => p.PublishShipmentStatusChangedAsync(It.IsAny<ShipmentStatusChangedEvent>()), Times.Never);
        mockCacheService.Verify(c => c.SetShipmentStatusAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }
}
