using DeveloperStore.Domain.Common;

namespace DeveloperStore.Domain.Services;
public interface IDomainEventPublisher
{
    Task PublishAsync(IDomainEvent domainEvent);
}