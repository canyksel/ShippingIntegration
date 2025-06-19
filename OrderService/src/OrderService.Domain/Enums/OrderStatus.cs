using System.ComponentModel;

namespace OrderService.Domain.Enums;

public enum OrderStatus
{
    [Description("Sipariş alındı, ödeme bekleniyor.")]
    Pending = 1,
    [Description("Ödeme tamamlandı.")]
    Paid = 2,
    [Description("Kargo firması teslim aldı.")]
    Shipped = 3,
    [Description("Müşteriye ulaştı.")]
    Delivered = 4,
    [Description("İptal edildi.")]
    Cancelled = 5
}