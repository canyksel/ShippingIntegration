using OrderService.Domain.Common;

namespace OrderService.Domain.Entities;

public class OrderProduct : EntityBase<Guid>
{
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public Guid OrderId { get; private set; }
    public virtual Order Order { get; private set; }
    public Guid ProductId { get; private set; }
    public virtual Product Product { get; private set; }

    protected OrderProduct() { }

    public OrderProduct(Order order, Product product, decimal unitPrice, int quantity)
    {
        Id = Guid.NewGuid();
        Order = order;
        OrderId = order.Id;
        Product = product;
        ProductId = product.Id;
        UnitPrice = unitPrice;
        Quantity = quantity;
        CreatedAt = DateTime.UtcNow;
    }

    public decimal TotalPrice => UnitPrice * Quantity;

    public void SetOrder(Order order)
    {
        Order = order;
        OrderId = order.Id;
    }
}