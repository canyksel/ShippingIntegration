namespace OrderService.Application.Orders.Queries.GetOrderByOrderNumber;

public partial class GetOrderByOrderNumberDto
{
    public Guid OrderId { get; set; }
    public string OrderNumber { get; set; }
    public string SellerName { get; set; }
    public string BuyerName { get; set; }
    public string ShippingCompanyName { get; set; }
    public string PaymentType { get; set; }
    public string Status { get; set; }

    public AddressDto Address { get; set; }
    public List<ProductDto> Products { get; set; }
}

public partial class AddressDto
{
    public string Summary { get; set; }
}

public partial class ProductDto
{
    public Guid ProductId { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}