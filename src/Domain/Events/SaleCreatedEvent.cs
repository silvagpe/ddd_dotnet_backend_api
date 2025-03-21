using DeveloperStore.Domain.Common;
using DeveloperStore.Domain.Entities;

namespace DeveloperStore.Domain.Events;

public class SaleCreatedEvent : IDomainEvent
{
    public Sale Sale { get; }
    public DateTime OccurredOn { get; }

    public SaleCreatedEvent(Sale sale)
    {
        Sale = sale;
        OccurredOn = DateTime.UtcNow;
    }
}