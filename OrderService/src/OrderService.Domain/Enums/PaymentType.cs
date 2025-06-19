using System.ComponentModel;

namespace OrderService.Domain.Enums;

public enum PaymentType
{
    [Description("Kredi Kartı")]
    CreditCard = 1,
    [Description("Banka Transferi")]
    BankTransfer = 2,
    [Description("Kapıda Ödeme")]
    CashOnDelivery = 3
}
