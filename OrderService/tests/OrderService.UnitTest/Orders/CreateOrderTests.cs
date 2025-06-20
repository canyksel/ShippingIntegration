using Moq;
using OrderService.Application.Orders.Commands.CreateOrder;
using OrderService.Domain.Entities;
using OrderService.Domain.Enums;
using OrderService.Domain.Repositories.Order;
using OrderService.Domain.Repositories.Product;
using OrderService.Domain.Repositories.ShippingCompany;

namespace OrderService.Unit.Test.Orders;

public class CreateOrderTests
{
    [Fact]
    public async Task HappyPath()
    {
        var shippingCompanyId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var mockOrderRepo = new Mock<IOrderRepository>();
        var mockShippingRepo = new Mock<IShippingCompanyRepository>();
        var mockProductRepo = new Mock<IProductRepository>();

        var shippingCompany = new ShippingCompany("MNG", "MNG001", new CompanyAddress("TR", "İstanbul", "Marmara", "34000", "Merkez", "Açık Adres"));

        var product = new Product("Laptop", "Gaming", "Asus", "Electronics", 10);
        mockShippingRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(shippingCompany);
        mockProductRepo.Setup(r => r.GetByIdAsync(productId)).ReturnsAsync(product);
        mockOrderRepo.Setup(r => r.UnitOfWork.SaveEntitesAsync(It.IsAny<CancellationToken>()))
             .ReturnsAsync(true);

        var handler = new CreateOrderCommandHandler(mockOrderRepo.Object, mockShippingRepo.Object, mockProductRepo.Object);

        var command = new CreateOrderCommand
        {
            SellerName = "TechStore",
            BuyerName = "John Doe",
            PaymentType = PaymentType.CreditCard,
            ShippingCompanyId = shippingCompanyId,
            Address = new OrderAddressDto
            {
                Country = "TR",
                City = "İstanbul",
                State = "Marmara",
                PostalCode = "34000",
                AddressTitle = "Ev",
                AddressDetail = "Ev adresi detay"
            },
            Products = new List<OrderProductDto>
            {
                new() { ProductId = productId, Quantity = 2, UnitPrice = 1000 },
                new() { ProductId = productId, Quantity = 2, UnitPrice = 1000 },
                new() { ProductId = productId, Quantity = 2, UnitPrice = 1000 }
            }
        };

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, result.OrderId);
        Assert.StartsWith("ORD-", result.OrderNumber);
    }

    [Fact]
    public async Task ShouldThrowExceptionWhenShippingCompanyNotFound()
    {
        var mockOrderRepo = new Mock<IOrderRepository>();
        var mockShippingRepo = new Mock<IShippingCompanyRepository>();
        var mockProductRepo = new Mock<IProductRepository>();

        mockShippingRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                        .ReturnsAsync((OrderService.Domain.Entities.ShippingCompany?)null);

        var handler = new CreateOrderCommandHandler(
            mockOrderRepo.Object,
            mockShippingRepo.Object,
            mockProductRepo.Object
        );

        var command = new CreateOrderCommand
        {
            SellerName = "Store",
            BuyerName = "User",
            ShippingCompanyId = Guid.NewGuid(),
            Address = new OrderAddressDto
            {
                Country = "TR",
                City = "İstanbul",
                State = "Marmara",
                PostalCode = "34000",
                AddressTitle = "Ev",
                AddressDetail = "Adres"
            },
            PaymentType = Domain.Enums.PaymentType.CreditCard,
            Products = new List<OrderProductDto>()
        };

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.Handle(command, CancellationToken.None));

        Assert.Equal("Shipping company not found.", ex.Message);
    }
}
