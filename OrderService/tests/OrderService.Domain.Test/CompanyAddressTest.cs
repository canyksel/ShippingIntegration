using OrderService.Domain.Entities;

namespace OrderService.Domain.Test;

public class CompanyAddressTest
{
    [Fact]
    public void ShouldCreateCompanyAddressSuccessfully()
    {
        var address = new CompanyAddress(
            "Türkiye",
            "İstanbul",
            "Marmara",
            "34000",
            "Ofis",
            "Büyükdere Cad. No:1");

        Assert.Equal("Türkiye", address.Country);
        Assert.Equal("İstanbul", address.City);
        Assert.Equal("Marmara", address.State);
        Assert.Equal("34000", address.PostalCode);
        Assert.Equal("Ofis", address.AddressTitle);
        Assert.Equal("Büyükdere Cad. No:1", address.AddressDetail);

        var expectedSummary = "Ofis: Büyükdere Cad. No:1, İstanbul, Marmara, 34000, Türkiye";
        Assert.Equal(expectedSummary, address.AddressSummary);
    }
}
