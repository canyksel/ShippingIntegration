using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Persistence;

public static class SeedData
{
    public static async Task SeedAsync(OrderContext context)
    {
        if (!await context.ShippingCompanies.AnyAsync())
        {
            var companies = new List<ShippingCompany>
            {
                new(
                    name: "Yurtiçi Kargo",
                    code: "YURTICI",
                    address: new CompanyAddress("Türkiye", "İstanbul", "Marmara", "34000", "Merkez", "Yurtiçi Kargo Genel Müdürlük")
                ),
                new(
                    name: "Aras Kargo",
                    code: "ARAS",
                    address: new CompanyAddress("Türkiye", "Ankara", "İç Anadolu", "06000", "Merkez", "Aras Kargo Genel Müdürlük")
                ),
                new(
                    name: "MNG Kargo",
                    code: "MNG",
                    address: new CompanyAddress("Türkiye", "İzmir", "Ege", "35000", "Merkez", "MNG Kargo Genel Müdürlük")
                )
            };
            await context.ShippingCompanies.AddRangeAsync(companies);
        }

        if (!await context.Products.AnyAsync())
        {
            var products = new List<Product>
            {
                new("Laptop", "Açıklama 1", "Monster", "Bilgisayar", 500),
                new("Telefon", "Açıklama 2", "Apple", "Elektronik", 100),
                new("Laptop", "Açıklama 3", "Lenovo", "Bilgisayar", 50),
                new("Laptop", "Açıklama 4", "HP", "Bilgisayar", 50),
            };
            await context.Products.AddRangeAsync(products);
        }

        await context.SaveChangesAsync();
    }
}