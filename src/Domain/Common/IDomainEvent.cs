namespace DeveloperStore.Domain.Common;
public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}