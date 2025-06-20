using OrderService.Domain.Common;

namespace OrderService.Domain.Entities;

public class ShippingCompany : EntityBase<Guid>
{
    public string Name { get; private set; }
    public string Code { get; private set; }
    public DateTime? ShipmentDate { get; private set; }
    public Guid CompanyAddressId { get; private set; }
    public virtual CompanyAddress Address { get; private set; }

    protected ShippingCompany() { }

    public ShippingCompany(string name, string code, CompanyAddress address, DateTime? shipmentDate = null)
    {
        Id = Guid.NewGuid();
        Name = name;
        Code = code;
        Address = address;
        ShipmentDate = shipmentDate;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string code, CompanyAddress address)
    {
        Name = name;
        Code = code;
        Address = address;
        SetUpdated();
    }

    public void ScheduleShipment(DateTime shipmentDate)
    {
        ShipmentDate = shipmentDate;
        SetUpdated();
    }
}