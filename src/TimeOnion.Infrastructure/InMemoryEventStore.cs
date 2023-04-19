using TimeOnion.Domain.BuildingBlocks;

namespace TimeOnion.Infrastructure;

public class InMemoryEventStore : IEventStore
{
    private readonly List<IDomainEvent> _domainEvents = new();

    public Task<IReadOnlyCollection<IDomainEvent>> GetAll() =>
        Task.FromResult<IReadOnlyCollection<IDomainEvent>>(_domainEvents);

    public Task AddRange(IEnumerable<IDomainEvent> domainEvents)
    {
        _domainEvents.AddRange(domainEvents);

        return Task.CompletedTask;
    }
}