using MyRecipeBook.Domain.Enum;

namespace MyRecipeBook.Domain.Entities;

public class Ingredients : EntityBase
{
    public string Name { get; set; }
    public double? Quantity { get; set; }
    public UnitMeasure UnitMeasure { get; set; }
    public Guid IngredientsIdentifier { get; set; }
    public long RecipeId { get; set; }

}
