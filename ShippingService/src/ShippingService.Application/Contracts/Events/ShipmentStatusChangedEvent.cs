namespace ShippingService.Application.Contracts.Events;

public class ShipmentStatusChangedEvent
{
    public string OrderNumber { get; set; }
    public string NewStatus { get; set; }
}