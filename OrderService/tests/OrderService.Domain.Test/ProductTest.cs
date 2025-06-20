using OrderService.Domain.Entities;

namespace OrderService.Domain.Test;

public class ProductTest
{
    [Fact]
    public void ShouldCreateProductSuccessfully()
    {
        var product = new Product("Laptop", "Güçlü bir cihaz", "Monster", "Bilgisayar", 100);

        Assert.Equal("Laptop", product.Name);
        Assert.Equal("Güçlü bir cihaz", product.Description);
        Assert.Equal("Monster", product.BrandName);
        Assert.Equal("Bilgisayar", product.CategoryName);
        Assert.Equal(100, product.Stock);
    }

    [Fact]
    public void ShouldDecreaseStockSuccessfully()
    {
        var product = new Product("Monster Tulpar", "Oyuncu bilgisayarı", "Monster", "Elektronik", 10);

        product.DecreaseStock(3);

        Assert.Equal(7, product.Stock);
    }

    [Fact]
    public void ShouldThrowWhenDecreaseStockExceedsStock()
    {
        var product = new Product("Tablet", "Açıklama", "Samsung", "Elektronik", 2);

        var ex = Assert.Throws<InvalidOperationException>(() => product.DecreaseStock(5));
        Assert.Equal("Stok yetersiz.", ex.Message);
    }

}
