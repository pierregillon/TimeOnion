using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.List;

public record TodoListState(
    TimeHorizons CurrentTimeHorizon,
    IEnumerable<TodoListReadModel> TodoLists,
    CategoryVisibility CurrentCategoryVisibility
) : IState
{
    // ReSharper disable once UnusedMember.Global
    public static TodoListState Initialize() =>
        new(
            TimeHorizons.ThisDay,
            new List<TodoListReadModel>(),
            CategoryVisibility.ShowAll
        );
}

public enum CategoryVisibility
{
    ShowAll,
    HideWithoutItems,
    HideAll
}