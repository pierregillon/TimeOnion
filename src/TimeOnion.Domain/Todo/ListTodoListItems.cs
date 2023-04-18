using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.List;

namespace TimeOnion.Domain.Todo;

public record ListTodoListItemsQuery(Temporality Temporality) : IQuery<IReadOnlyCollection<TodoListItem>>;

internal class ListTodoListItemsQueryHandler : IQueryHandler<ListTodoListItemsQuery, IReadOnlyCollection<TodoListItem>>,
    IDomainEventListener<TodoItemAdded>,
    IDomainEventListener<TodoItemCompleted>,
    IDomainEventListener<ItemReadyTodo>,
    IDomainEventListener<TodoItemDescriptionFixed>,
    IDomainEventListener<TodoItemRescheduled>,
    IDomainEventListener<TodoItemDeleted>
{
    private readonly IReadModelDatabase _database;

    public ListTodoListItemsQueryHandler(IReadModelDatabase database) => _database = database;

    public async Task On(ItemReadyTodo domainEvent) => await _database.Update<TodoListItem>(
        x => x.Id == domainEvent.ItemId.Value,
        item => item with { IsDone = false }
    );

    public async Task On(TodoItemAdded domainEvent)
    {
        var newItem = new TodoListItem(
            domainEvent.ItemId.Value,
            domainEvent.Description.Value,
            false,
            domainEvent.Temporality,
            false
        );
        await _database.Add(newItem);
    }

    public async Task On(TodoItemCompleted domainEvent) => await _database.Update<TodoListItem>(
        x => x.Id == domainEvent.TodoItemId.Value,
        item => item with { IsDone = true }
    );

    public async Task On(TodoItemDeleted domainEvent) => await _database.Update<TodoListItem>(
        x => x.Id == domainEvent.ItemId.Value,
        item => item with { IsDeleted = true }
    );

    public async Task On(TodoItemDescriptionFixed domainEvent) => await _database.Update<TodoListItem>(
        x => x.Id == domainEvent.ItemId.Value,
        item => item with { Description = domainEvent.NewItemDescription.Value }
    );

    public async Task On(TodoItemRescheduled domainEvent) => await _database.Update<TodoListItem>(
        x => x.Id == domainEvent.ItemId.Value,
        item => item with { Temporality = domainEvent.NewTemporality }
    );

    public async Task<IReadOnlyCollection<TodoListItem>> Handle(ListTodoListItemsQuery query) =>
        (await _database.GetAll<TodoListItem>())
        .Where(x => x.IsDeleted == false)
        .Where(x => x.Temporality == query.Temporality)
        .ToArray();
}