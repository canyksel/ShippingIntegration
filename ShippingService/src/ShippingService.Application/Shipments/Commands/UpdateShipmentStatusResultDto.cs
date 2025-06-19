namespace ShippingService.Application.Shipments.Commands;

public class UpdateShipmentStatusResultDto
{
    public Guid ShipmentId { get; set; }
    public Guid OrderId { get; set; }
    public string OrderNumber { get; set; }
    public string UpdatedStatus { get; set; }
}