using Microsoft.Extensions.Logging;
using Moq;
using ShippingService.Application.Common;
using ShippingService.Application.Shipments.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Unit.Test.Shipments;

public class TrackShipmentTest
{
    [Fact]
    public async Task HappyPath()
    {
        var expectedStatus = "InTransit";
        var orderNumber = "ORD123";

        var mockCacheService = new Mock<IShipmentCacheService>();
        mockCacheService
            .Setup(x => x.GetShipmentStatusAsync(orderNumber))
            .ReturnsAsync(expectedStatus);

        var mockLogger = new Mock<ILogger<TrackShipmentQueryHandler>>();

        var handler = new TrackShipmentQueryHandler(mockCacheService.Object, mockLogger.Object);
        var query = new TrackShipmentQuery { OrderNumber = orderNumber };

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.Equal(expectedStatus, result);
        mockLogger.Verify(x => x.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains(orderNumber)),
            null,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }

    [Fact]
    public async Task ShouldReturnNullWhenShipmentNotInCache()
    {
        var orderNumber = "ORD999";

        var mockCacheService = new Mock<IShipmentCacheService>();
        mockCacheService
            .Setup(x => x.GetShipmentStatusAsync(orderNumber))
            .ReturnsAsync((string?)null);

        var mockLogger = new Mock<ILogger<TrackShipmentQueryHandler>>();

        var handler = new TrackShipmentQueryHandler(mockCacheService.Object, mockLogger.Object);
        var query = new TrackShipmentQuery { OrderNumber = orderNumber };

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.Null(result);
    }
}
