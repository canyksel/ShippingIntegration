using OrderService.Domain.Common;

namespace OrderService.Domain.Entities;

public class Product : EntityBase<Guid>
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string BrandName { get; private set; }
    public string CategoryName { get; private set; }
    public int Stock { get; private set; }

    protected Product() { }

    public Product(string name, string description, string brandName, string categoryName, int stock)
    {
        Id = Guid.NewGuid();
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        BrandName = brandName ?? throw new ArgumentNullException(nameof(brandName));
        CategoryName = categoryName ?? throw new ArgumentNullException(nameof(categoryName));
        Stock = stock;
        CreatedAt = DateTime.UtcNow;
    }

    public void DecreaseStock(int quantity)
    {
        if (quantity > Stock)
            throw new InvalidOperationException("Stok yetersiz.");

        Stock -= quantity;
        SetUpdated();
    }

    public void IncreaseStock(int quantity)
    {
        Stock += quantity;
        SetUpdated();
    }
}