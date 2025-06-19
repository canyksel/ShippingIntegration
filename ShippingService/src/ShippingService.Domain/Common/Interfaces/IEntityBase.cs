namespace ShippingService.Domain.Common.Interfaces;

public interface IEntityBase { }

public interface IEntityBase<TId> : IEntityBase
{
    TId Id { get; }
}