using DeveloperStore.Domain.Common;
using DeveloperStore.Domain.Entities;

namespace DeveloperStore.Domain.Events;

public class SaleCancelledEvent : IDomainEvent
    {
        public Sale Sale { get; }
        public DateTime OccurredOn { get; }

        public SaleCancelledEvent(Sale sale)
        {
            Sale = sale;
            OccurredOn = DateTime.UtcNow;
        }
    }