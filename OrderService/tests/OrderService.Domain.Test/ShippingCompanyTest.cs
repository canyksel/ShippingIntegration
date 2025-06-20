using OrderService.Domain.Entities;

namespace OrderService.Domain.Test;

public class ShippingCompanyTest
{
    [Fact]
    public void ShouldCreateShippingCompanySuccessfully()
    {
        // Arrange
        var address = new CompanyAddress("Türkiye", "İstanbul", "Marmara", "34000", "Merkez", "Büyükdere Cad.");
        var company = new ShippingCompany("Yurtiçi Kargo", "YURTICI", address);

        // Assert
        Assert.Equal("Yurtiçi Kargo", company.Name);
        Assert.Equal("YURTICI", company.Code);
        Assert.Equal(address, company.Address);
        Assert.Equal(address.Id, company.CompanyAddressId);
        Assert.Null(company.ShipmentDate);
    }
}