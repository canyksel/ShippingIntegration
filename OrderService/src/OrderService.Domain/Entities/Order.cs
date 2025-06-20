using OrderService.Domain.Common;
using OrderService.Domain.Enums;

namespace OrderService.Domain.Entities;

public class Order : EntityBase<Guid>
{
    public string OrderNumber { get; private set; }
    public string SellerName { get; private set; }
    public string BuyerName { get; private set; }
    public PaymentType PaymentType { get; private set; }
    public OrderStatus Status { get; private set; }
    public Guid OrderAddressId { get; private set; }
    public virtual OrderAddress Address { get; private set; }
    public virtual ICollection<OrderProduct> Products { get; private set; }

    public Guid ShippingCompanyId { get; private set; }
    public virtual ShippingCompany ShippingCompany { get; private set; }

    protected Order() { }

    public Order(
    string sellerName,
    string buyerName,
    OrderAddress address,
    PaymentType paymentType,
    ShippingCompany shippingCompany,
    IEnumerable<OrderProduct> products)
    {
        Id = Guid.NewGuid();
        OrderNumber = GenerateOrderNumber();
        SellerName = sellerName ?? throw new ArgumentNullException(nameof(sellerName));
        BuyerName = buyerName ?? throw new ArgumentNullException(nameof(buyerName));
        Address = address ?? throw new ArgumentNullException(nameof(address));
        OrderAddressId = address.Id;
        Status = OrderStatus.Pending;
        PaymentType = paymentType;
        ShippingCompany = shippingCompany ?? throw new ArgumentNullException(nameof(shippingCompany));
        ShippingCompanyId = shippingCompany.Id;
        CreatedAt = DateTime.UtcNow;

        Products = new List<OrderProduct>();

        if (products is not null)
        {
            foreach (var product in products)
            {
                AddProduct(product);
            }

        }

    }

    public void UpdateOrderStatus(OrderStatus status)
    {
        Status = status;
    }

    public void AddProduct(OrderProduct product)
    {
        if (product == null) throw new ArgumentNullException(nameof(product));
        Products.Add(product);
    }

    public void ClearProducts()
    {
        Products.Clear();
    }

    public decimal TotalAmount => Products.Sum(p => p.TotalPrice);

    private static string GenerateOrderNumber()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        var suffix = new string(Enumerable.Repeat(chars, 12)
            .Select(s => s[random.Next(s.Length)]).ToArray());

        return $"ORD-{suffix}";
    }
}