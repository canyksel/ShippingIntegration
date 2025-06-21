using Microsoft.Extensions.Logging;
using Moq;
using OrderService.Application.Orders.Commands.CancelOrder;
using OrderService.Domain.Entities;
using OrderService.Domain.Enums;
using OrderService.Domain.Repositories.Order;
using System.Linq.Expressions;
using System.Reflection;

namespace OrderService.Unit.Test.Orders;

public class CancelOrderTests
{
    [Fact]
    public async Task HappyPath()
    {
        var mockOrderRepository = new Mock<IOrderRepository>();
        var mockLogger = new Mock<ILogger<CancelOrderCommandHandler>>();

        var mockOrder = new Mock<Order>();

        mockOrderRepository.Setup(r => r.GetByOrderNumberWithDetailsAsync(mockOrder.Object.OrderNumber))
                           .ReturnsAsync(mockOrder.Object);

        mockOrderRepository.Setup(r => r.UnitOfWork.SaveChangesAsync(It.IsAny<CancellationToken>()))
                           .ReturnsAsync(1);

        var handler = new CancelOrderCommandHandler(mockOrderRepository.Object, mockLogger.Object);

        var result = await handler.Handle(new CancelOrderCommand { OrderNumber = mockOrder.Object.OrderNumber }, default);

        result.Equals(1);
        mockOrderRepository.Verify(r => r.Update(It.Is<Order>(o => o.Status == OrderStatus.Cancelled)), Times.Once);
    }

    [Fact]
    public async Task ShouldThrowExceptionWhenOrderNotFound()
    {
        var mockOrderRepository = new Mock<IOrderRepository>();
        var mockLogger = new Mock<ILogger<CancelOrderCommandHandler>>();

        mockOrderRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Order, bool>>>()))
                           .ReturnsAsync((Order)null);

        var handler = new CancelOrderCommandHandler(mockOrderRepository.Object, mockLogger.Object);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.Handle(new CancelOrderCommand { OrderNumber = "ORD-999" }, default));
    }

    [Fact]
    public async Task ShouldThrowExceptionWhenOrderIsNotCancellable()
    {
        var mockOrderRepository = new Mock<IOrderRepository>();
        var mockLogger = new Mock<ILogger<CancelOrderCommandHandler>>();

        var order = CreateOrder();

        var statusField = typeof(Order).GetProperty("Status", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        statusField!.SetValue(order, OrderStatus.Shipped);

        mockOrderRepository.Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Order, bool>>>()))
                           .ReturnsAsync(order);

        var handler = new CancelOrderCommandHandler(mockOrderRepository.Object, mockLogger.Object);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.Handle(new CancelOrderCommand { OrderNumber = order.OrderNumber }, default));
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
