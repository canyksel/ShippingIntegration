using OrderService.Domain.Entities;
using OrderService.Domain.Enums;

namespace OrderService.Domain.Test;

public class OrderProductTest
{
    [Fact]
    public void ShouldCreateOrderProductSuccessfully()
    {
        var product = new Product("Laptop", "Güçlü bir cihaz", "Monster", "Bilgisayar", 10);
        var address = new OrderAddress("TR", "İstanbul", "Marmara", "34000", "Ev", "Açık adres");
        var companyAddress = new CompanyAddress("TR", "İstanbul", "Marmara", "34000", "Ofis", "Adres");
        var shipping = new ShippingCompany("Yurtiçi", "YURTICI", companyAddress);
        var order = new Order("Satıcı", "Alıcı", address, PaymentType.CreditCard, shipping, new List<OrderProduct>());

        var orderProduct = new OrderProduct(order, product, 15000m, 2);

        Assert.Equal(order.Id, orderProduct.OrderId);
        Assert.Equal(product.Id, orderProduct.ProductId);
        Assert.Equal(2, orderProduct.Quantity);
        Assert.Equal(15000m, orderProduct.UnitPrice);
        Assert.Equal(30000m, orderProduct.TotalPrice);
    }
}
