using BlazorState;
using TimeOnion.Domain.Todo.List;

namespace TimeOnion.Actions;

public class TodoListState : State<TodoListState>
{
    public string NewTodoItemDescription { get; set; } = string.Empty;
    public TimeHorizons CurrentTimeHorizons { get; set; } = TimeHorizons.ThisDay;
    public IEnumerable<TodoListReadModel> TodoLists { get; set; } = Array.Empty<TodoListReadModel>();

    public override void Initialize()
    {
    }

    public record LoadData : IAction;

    public record CreateNewTodoList : IAction;

    public record AddNewItem(TodoListId ListId, string? Text) : IAction;

    public record MarkItemAsDone(TodoListId ListId, TodoItemId ItemId) : IAction;

    public record MarkItemAsToDo(TodoListId ListId, TodoItemId ItemId) : IAction;

    public record FixItemDescription(TodoListId ListId, TodoItemId ItemId, string NewDescription) : IAction;

    public record RescheduleTodoItem(TodoListId ListId, TodoItemId ItemId, TimeHorizons TimeHorizons) : IAction;

    public record ChangeCurrentTemporality(TimeHorizons TimeHorizons) : IAction;

    public record DeleteItem(TodoListId ListId, TodoItemId ItemId) : IAction;

    public record RenameTodoList(TodoListId ListId, string NewName) : IAction;

    public record DeleteTodoList(TodoListId ListId) : IAction;
}