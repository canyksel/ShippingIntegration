using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Infrastructure.Persistence;

public static class SeedData
{
    public static async Task SeedAsync(OrderContext context)
    {
        if (!await context.ShippingCompanies.AnyAsync())
        {
            var companies = new List<ShippingCompany>
            {
                new("Yurtiçi Kargo", "YURTICI", null, null),
                new("Aras Kargo", "ARAS", null, null),
                new("MNG Kargo", "MNG", null, null)
            };
            await context.ShippingCompanies.AddRangeAsync(companies);
        }

        if (!await context.Products.AnyAsync())
        {
            var products = new List<Product>
            {
                new("Laptop", "Açıklama 4", "Monster", "Bilgisayar", 500),
                new("Telefon", "Açıklama 1", "Apple", "Elektronik", 100),
                new("Laptop", "Açıklama 2", "Lenovo", "Bilgisayar", 50),
                new("Laptop", "Açıklama 3", "HP", "Bilgisayar", 50),
            };
            await context.Products.AddRangeAsync(products);
        }

        await context.SaveChangesAsync();
    }
}