using MediatR;
using OrderService.Domain.Common.Interfaces;

namespace OrderService.Infrastructure.Eventing;

public class DomainEventDispatcher(IMediator mediator) : IDomainEventDispatcher
{
    public async Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        if (domainEvent is null)
            throw new ArgumentNullException(nameof(domainEvent));

        await mediator.Publish(domainEvent, cancellationToken);
    }
}