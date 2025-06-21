using MediatR;
using ShippingService.Domain.Common.Interfaces;

namespace ShippingService.Infrastructure.Eventing;

public class DomainEventDispatcher(IMediator mediator) : IDomainEventDispatcher
{
    public async Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        if (domainEvent is null)
            throw new ArgumentNullException(nameof(domainEvent));

        await mediator.Publish(domainEvent, cancellationToken);
    }
}
