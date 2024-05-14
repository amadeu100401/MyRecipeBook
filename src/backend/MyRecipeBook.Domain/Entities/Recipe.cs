using MyRecipeBook.Domain.Enum;

namespace MyRecipeBook.Domain.Entities;

public class Recipe : EntityBase
{
    public Recipe()
    {
        Ingredients = new();
    }

    public string Title { get; set; }
    public Category Category { get; set; }
    public string MethodPrepar { get; set; }
    public double TimePreparationMinutes { get; set; }
    public List<Ingredients> Ingredients { get; set; }
    public Guid RecipeIdentifier { get; set; }
    public long UserId { get; set; }
}
