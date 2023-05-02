using BlazorState;
using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Domain.Todo.UseCases.Positionning;

namespace TimeOnion.Pages.TodayTaskPreparation.Actions;

public class RepositionItemActionHandler : ActionHandler<TodoListState.RepositionItemAboveAnother>
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public RepositionItemActionHandler(
        IStore aStore,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(aStore)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    public override async Task Handle(
        TodoListState.RepositionItemAboveAnother aAction,
        CancellationToken aCancellationToken
    )
    {
        var state = Store.GetState<TodoListState>();

        var command =
            new RepositionItemAboveAnotherCommand(
                aAction.ListId,
                aAction.ItemId,
                aAction.ReferenceItemId
            );

        await _commandDispatcher.Dispatch(command);

        state.TodoListItems[aAction.ListId] =
            await _queryDispatcher.Dispatch(new ListTodoItemsQuery(aAction.ListId, state.CurrentTimeHorizon));
    }
}