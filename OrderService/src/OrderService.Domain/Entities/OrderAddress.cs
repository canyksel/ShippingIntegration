using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.Entities;

public class OrderAddress
{
    public string Country { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string PostalCode { get; private set; }
    public string AddressTitle { get; private set; }
    public string AddressDetail { get; private set; }
    public string AddressSummary => $"{AddressTitle}: {AddressDetail}, {City}, {State}, {PostalCode}, {Country}";

    protected OrderAddress() { }

    public OrderAddress(
    string country,
    string city,
    string state,
    string postalCode,
    string addressTitle,
    string addressDetail)
    {
        Country = country ?? throw new ArgumentNullException(nameof(country));
        City = city ?? throw new ArgumentNullException(nameof(city));
        State = state ?? throw new ArgumentNullException(nameof(state));
        PostalCode = postalCode ?? throw new ArgumentNullException(nameof(postalCode));
        AddressTitle = addressTitle ?? throw new ArgumentNullException(nameof(addressTitle));
        AddressDetail = addressDetail ?? throw new ArgumentNullException(nameof(addressDetail));
    }
}
