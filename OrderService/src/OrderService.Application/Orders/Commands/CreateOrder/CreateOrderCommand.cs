using MediatR;
using OrderService.Domain.Enums;

namespace OrderService.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommand : IRequest<CreateOrderResultDto>
{
    public string SellerName { get; set; }
    public string BuyerName { get; set; }
    public PaymentType PaymentType { get; set; }
    public Guid ShippingCompanyId { get; set; }
    public OrderAddressDto Address { get; set; }
    public List<OrderProductDto> Products { get; set; }
}

public class CreateOrderResultDto
{
    public Guid OrderId { get; set; }
    public string OrderNumber { get; set; }
    public string PaymentType { get; set; }
    public string OrderStatus { get; set; }
}