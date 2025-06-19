using MediatR;
using OrderService.Domain.Common.Interfaces;

namespace OrderService.Domain.Common;

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