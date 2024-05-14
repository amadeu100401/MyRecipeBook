namespace MyRecipeBook.Application.UseCases.Recipe.GetUnitMeasure;

public interface IGetUnitMeasureUseCase
{
    public Task<IDictionary<string, string>> Execute();
}
