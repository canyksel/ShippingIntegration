using System.ComponentModel;

namespace ShippingService.Domain.Enums;

public enum ShipmentStatus
{
    [Description("Hazırlanıyor.")]
    Prepared = 1,
    [Description("Yolda.")]
    InTransit = 2,
    [Description("Teslim Edildi.")]
    Delivered = 3
}