using DeveloperStore.Domain.Common;
using DeveloperStore.Domain.Services;
using Microsoft.Extensions.Logging;

namespace DeveloperStore.Infrastructure.Services;

public class DomainEventPublisher : IDomainEventPublisher
{
    private readonly ILogger<DomainEventPublisher> _logger;

    public DomainEventPublisher(ILogger<DomainEventPublisher> logger)
    {
        _logger = logger;
    }

    public Task PublishAsync(IDomainEvent domainEvent)
    {
        // TODO: Implement the logic to publish the domain event        
        // For now, we will just log the event for demonstration purposes

        _logger.LogInformation("Publishing domain event: {DomainEvent}", domainEvent.GetType().Name);
        
        return Task.CompletedTask;        
    }
}