using OrderService.Domain.Enums;

namespace OrderService.Application.Orders.Commands.CreateOrder;

public partial class CreateOrderDto
{
    public string SellerName { get; set; }
    public string BuyerName { get; set; }
    public PaymentType PaymentType { get; set; }
    public Guid ShippingCompanyId { get; set; }

    public OrderAddressDto Address { get; set; }
    public List<OrderProductDto> Products { get; set; }
}

public partial class OrderProductDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

public partial class OrderAddressDto
{
    public string Country { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string PostalCode { get; set; }
    public string AddressTitle { get; set; }
    public string AddressDetail { get; set; }
    public string AddressSummary { get; set; }
}