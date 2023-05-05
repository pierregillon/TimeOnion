using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Pages.TodayTaskPreparation.Steps;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodayTaskPreparation.Actions;

public class LoadTodayTaskPreparationActionHandler :
    ActionHandlerBase<TodayTaskPreparationState, TodayTaskPreparationState.Load>
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public LoadTodayTaskPreparationActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    protected override async Task<TodayTaskPreparationState> Apply(
        TodayTaskPreparationState state,
        TodayTaskPreparationState.Load action
    )
    {
        var items = (await Dispatch(new ListYesterdayUndoneTasksQuery()))
            .Select(SelectableTodoItem.From)
            .ToArray();

        return state with
        {
            CurrentStep = new EndYesterdayTasksStep(_queryDispatcher, _commandDispatcher),
            YesterdayUndoneTasks = items
        };
    }
}