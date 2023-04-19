using NSubstitute;
using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.List;
using TimeOnion.Infrastructure;

namespace TimeOnion.Tests.Unit;

public class CachedEventStoreTests
{
    private readonly IEventStore _decorated;
    private readonly CachedEventStore _cache;

    public CachedEventStoreTests()
    {
        _decorated = Substitute.For<IEventStore>();
        _cache = new CachedEventStore(_decorated, new DomainEventsCache());
    }

    [Fact]
    public async Task Getting_all_domain_events_the_first_time_do_real_call()
    {
        await _cache.GetAll();

        await _decorated
            .Received(1)
            .GetAll();
    }

    [Fact]
    public async Task Getting_all_domain_events_after_first_call_does_not_call_decorated()
    {
        await _cache.GetAll();

        _decorated.ClearReceivedCalls();

        await _cache.GetAll();

        await _decorated
            .Received(0)
            .GetAll();
    }

    [Fact]
    public async Task Saving_new_domain_events_does_not_call_decorated()
    {
        await _cache.Save(new[]
        {
            new TodoItemAdded(TodoItemId.New(), new ItemDescription("test"), Temporality.ThisMonth)
        });

        await _decorated
            .Received(0)
            .Save(Arg.Any<IReadOnlyCollection<IDomainEvent>>());
    }

    [Fact]
    public async Task Saving_uncommitted_events_calls_decorated()
    {
        var todoItemAdded = new TodoItemAdded(TodoItemId.New(), new ItemDescription("test"), Temporality.ThisMonth);

        await _decorated.Save(new[]
        {
            todoItemAdded
        });

        await _cache.SaveUncommittedEvents();

        await _decorated
            .Received(1)
            .Save(Arg.Is<IReadOnlyCollection<IDomainEvent>>(x => x.Single().Equals(todoItemAdded)));
    }
}