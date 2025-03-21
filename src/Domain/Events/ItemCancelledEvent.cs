using DeveloperStore.Domain.Common;
using DeveloperStore.Domain.Entities;

namespace DeveloperStore.Domain.Events;

public class ItemCancelledEvent : IDomainEvent
    {
        public Sale Sale { get; }
        public int ProductId { get; }
        public DateTime OccurredOn { get; }

        public ItemCancelledEvent(Sale sale, int productId)
        {
            Sale = sale;
            ProductId = productId;
            OccurredOn = DateTime.UtcNow;
        }
    }