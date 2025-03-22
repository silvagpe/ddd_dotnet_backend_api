using DeveloperStore.Domain.Common;
using DeveloperStore.Domain.Entities;

namespace DeveloperStore.Domain.Events;

public class ItemCancelledEvent : IDomainEvent
    {
        public Sale Sale { get; }
        public long ProductId { get; }
        public DateTime OccurredOn { get; }

        public ItemCancelledEvent(Sale sale, long productId)
        {
            Sale = sale;
            ProductId = productId;
            OccurredOn = DateTime.UtcNow;
        }
    }