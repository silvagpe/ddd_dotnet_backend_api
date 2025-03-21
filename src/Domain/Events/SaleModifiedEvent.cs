using DeveloperStore.Domain.Common;
using DeveloperStore.Domain.Entities;

namespace DeveloperStore.Domain.Events;

public class SaleModifiedEvent : IDomainEvent
    {
        public Sale Sale { get; }
        public DateTime OccurredOn { get; }

        public SaleModifiedEvent(Sale sale)
        {
            Sale = sale;
            OccurredOn = DateTime.UtcNow;
        }
    }