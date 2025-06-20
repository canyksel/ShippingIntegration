using OrderService.Domain.Entities;
using OrderService.Domain.Enums;

namespace OrderService.Domain.Test;

public class OrderTest
{

    [Fact]
    public void ShouldCreateOrderSuccessfully()
    {
        var companyAddress = new CompanyAddress("Türkiye", "İstanbul", "Marmara", "34000", "Ofis", "Büyükdere Cad. No:1");
        var address = new OrderAddress("TR", "İstanbul", "Marmara", "34000", "Ev", "Açık adres");
        var shipping = new ShippingCompany("Yurtiçi", "YURT", companyAddress);
        var product = new Product("Laptop", "...", "Monster", "Bilgisayar", 100);
        var order = new Order("Satıcı", "Alıcı", address, PaymentType.CreditCard, shipping, new List<OrderProduct>());

        var orderProduct = new OrderProduct(order, product, 10000, 2);
        order.AddProduct(orderProduct);

        Assert.Equal(OrderStatus.Pending, order.Status);
        Assert.Equal(20000, order.TotalAmount);
        Assert.Single(order.Products);
    }
}
