using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;
using TimeOnion.Shared.MVU.ActionHandling;

namespace TimeOnion.Pages.TodoListPage.Details.Actions.Items;

internal record AddNewItemTodoAfterItemAction(
    TodoListId ListId,
    TodoItemId ItemId,
    string NewDescription
) : TodoItemAction(ListId);

internal class AddNewItemTodoAfterItemActionHandler :
    ActionApplier<AddNewItemTodoAfterItemAction, TodoListDetailsState>
{
    public AddNewItemTodoAfterItemActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListDetailsState> Apply(
        AddNewItemTodoAfterItemAction action,
        TodoListDetailsState state
    )
    {
        var item = state.TodoListItems.Single(x => x.Id == action.ItemId);

        if (string.IsNullOrWhiteSpace(action.NewDescription))
        {
            var newElement = new TodoListItemReadModelBeingCreated(
                TodoItemId.New(),
                item.ListId,
                string.Empty,
                null,
                item.TimeHorizon,
                item.CategoryId
            );

            return state with
            {
                TodoListItems = state.TodoListItems
                    .InsertAfter(newElement, item)
                    .ToList()
            };
        }

        var command =
            new AddItemToDoCommand(
                action.ListId,
                TodoItemId.New(),
                new TodoItemDescription(action.NewDescription),
                item.TimeHorizon,
                item.CategoryId,
                item.Id
            );

        await Dispatch(command);

        var items = await Dispatch(new ListTodoItemsQuery(action.ListId, state.CurrentTimeHorizon));

        return state with
        {
            TodoListItems = items
        };
    }
}