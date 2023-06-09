using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories.Core;

namespace TimeOnion.Domain.Categories;

public record DeleteCategoryCommand(CategoryId CategoryId) : ICommand;

internal class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand>
{
    private readonly IRepository<Category, CategoryId> _repository;

    public DeleteCategoryCommandHandler(IRepository<Category, CategoryId> repository) => _repository = repository;

    public async Task Handle(DeleteCategoryCommand command)
    {
        var category = await _repository.Get(command.CategoryId);

        category.Delete();

        await _repository.Save(category);
    }
}