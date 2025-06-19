using OrderService.Domain.Common.Interfaces;

namespace OrderService.Domain.Common;

public abstract class EntityBase<TId> : IEntityBase<TId>
{
    public virtual TId Id { get; protected set; } = default!;
    public virtual DateTime CreatedAt { get; protected set; }
    public virtual DateTime? UpdatedAt { get; protected set; }
    public virtual DateTime? DeletedAt { get; protected set; }

    protected EntityBase()
    {
        CreatedAt = DateTime.UtcNow;
    }

    public void SetUpdated()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    public void SoftDelete()
    {
        DeletedAt = DateTime.UtcNow;
    }

    public bool IsDeleted() { return DeletedAt != null; }

    private List<IDomainEvent>? _domainEvents;
    public IReadOnlyCollection<IDomainEvent>? DomainEvents => _domainEvents?.AsReadOnly();

    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents ??= new List<IDomainEvent>();
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents?.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }
}