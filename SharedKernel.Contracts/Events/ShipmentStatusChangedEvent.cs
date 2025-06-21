namespace SharedKernel.Contracts.Events
{
    public class ShipmentStatusChangedEvent
    {
        public string OrderNumber { get; set; }
        public string NewStatus { get; set; }
    }
}