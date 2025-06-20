using OrderService.Domain.Entities;

namespace OrderService.Domain.Test;

public class OrderAddressTest
{
    [Fact]
    public void ShouldCreateOrderAddressSuccessfully()
    {
        var country = "Türkiye";
        var city = "İstanbul";
        var state = "Avrupa Yakası";
        var postalCode = "34000";
        var addressTitle = "Ev";
        var addressDetail = "Çiçek Sokak No:12";

        var address = new OrderAddress(country, city, state, postalCode, addressTitle, addressDetail);

        Assert.Equal(country, address.Country);
        Assert.Equal(city, address.City);
        Assert.Equal(state, address.State);
        Assert.Equal(postalCode, address.PostalCode);
        Assert.Equal(addressTitle, address.AddressTitle);
        Assert.Equal(addressDetail, address.AddressDetail);
        Assert.Contains("Ev: Çiçek Sokak", address.AddressSummary);
    }
}
