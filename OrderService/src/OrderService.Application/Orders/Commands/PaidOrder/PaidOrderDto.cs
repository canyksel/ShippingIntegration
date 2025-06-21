namespace OrderService.Application.Orders.Commands.PaidOrder;

public class PaidOrderDto
{
    public string OrderNumber { get; set; }
    public string PaymentType { get; set; }
    public string OrderStatus { get; set; }
}
