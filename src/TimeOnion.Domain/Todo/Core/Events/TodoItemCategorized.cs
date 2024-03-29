using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories.Core;

namespace TimeOnion.Domain.Todo.Core.Events;

public record TodoItemCategorized(
    TodoListId ListId,
    TodoItemId ItemId,
    CategoryId? PreviousCategoryId,
    CategoryId NewCategoryId
) : UserDomainEvent(ListId.Value);