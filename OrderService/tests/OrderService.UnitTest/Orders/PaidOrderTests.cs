using Microsoft.Extensions.Logging;
using Moq;
using OrderService.Application.Orders.Commands.PaidOrder;
using OrderService.Application.Orders.Events;
using OrderService.Domain.Entities;
using OrderService.Domain.Enums;
using OrderService.Domain.Repositories.Order;
using System.Linq.Expressions;
using System.Reflection;

namespace OrderService.Unit.Test.Orders;

public class PaidOrderTests
{
    [Fact]
    public async Task HappyPath()
    {
        var order = new Order(
            "Seller", "Buyer",
            new OrderAddress("TR", "İstanbul", "Marmara", "34000", "Ev", "Ev adresi"),
            PaymentType.CreditCard,
            new ShippingCompany("Yurtiçi", "YRT001", new CompanyAddress("TR", "Ankara", "İç Anadolu", "06000", "Ofis", "Adres")),
            new List<OrderProduct>());

        var mockOrderRepo = new Mock<IOrderRepository>();
        mockOrderRepo.Setup(r => r.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Order, bool>>>()))
                     .ReturnsAsync(order);
        mockOrderRepo.Setup(r => r.UnitOfWork.SaveEntitesAsync(It.IsAny<CancellationToken>()))
                     .ReturnsAsync(true);

        var mockEventPublisher = new Mock<IEventPublisher>();
        var mockLogger = new Mock<ILogger<PaidOrderCommandHandler>>();

        var handler = new PaidOrderCommandHandler(mockOrderRepo.Object, mockEventPublisher.Object, mockLogger.Object);

        var result = await handler.Handle(new PaidOrderCommand { OrderNumber = order.OrderNumber }, CancellationToken.None);

        Assert.True(result);
        mockEventPublisher.Verify(e => e.PublishOrderPaidAsync(It.IsAny<OrderPaidEvent>()), Times.Once);
    }

    [Fact]
    public async Task ShouldThrowExceptionWhenOrderNotFound()
    {
        var mockOrderRepo = new Mock<IOrderRepository>();
        var mockPublisher = new Mock<IEventPublisher>();
        var mockLogger = new Mock<ILogger<PaidOrderCommandHandler>>();

        var handler = new PaidOrderCommandHandler(mockOrderRepo.Object, mockPublisher.Object, mockLogger.Object);

        var command = new PaidOrderCommand { OrderNumber = "NON_EXISTENT_ORDER" };

        mockOrderRepo.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Order, bool>>>())).ReturnsAsync((Order)null!);

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));

        mockLogger.Verify(
            l => l.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Order not found")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task ShouldThrowExceptionWhenOrderStatusIsNotPending()
    {
        var mockOrderRepo = new Mock<IOrderRepository>();
        var mockPublisher = new Mock<IEventPublisher>();
        var mockLogger = new Mock<ILogger<PaidOrderCommandHandler>>();

        var order = CreateOrder();

        var statusField = typeof(Order).GetProperty("Status", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        statusField!.SetValue(order, OrderStatus.Shipped);

        var handler = new PaidOrderCommandHandler(mockOrderRepo.Object, mockPublisher.Object, mockLogger.Object);
        var command = new PaidOrderCommand { OrderNumber = "ORD-123", };

        mockOrderRepo.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Order, bool>>>()))
                     .ReturnsAsync(order);

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));

        mockLogger.Verify(
            l => l.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Order is not in Pending status")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    private Order CreateOrder()
    {
        var address = new OrderAddress("Türkiye", "İstanbul", "Marmara", "34000", "Ev", "Açık adres");
        var shippingCompany = new ShippingCompany("Aras", "ARAS", new CompanyAddress("TR", "İstanbul", "Marmara", "34000", "Merkez", "Açıklama"));
        var product = new Product("Test", "Test Açıklama", "Marka", "Kategori", 10);
        var order = new Order("Satıcı", "Alıcı", address, PaymentType.CreditCard, shippingCompany, new List<OrderProduct>());
        var orderProduct = new OrderProduct(order, product, 100, 1);

        order.AddProduct(orderProduct);

        return order;
    }
}