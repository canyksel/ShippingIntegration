using MediatR;
using ShippingService.Domain.Common.Interfaces;

namespace ShippingService.Domain.Common;

public class DomainEvent : IDomainEvent, INotification
{
    public Guid Id { get; }
    public DateTime OccurredOn { get; }

    public DomainEvent()
    {
        Id = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
    }
}