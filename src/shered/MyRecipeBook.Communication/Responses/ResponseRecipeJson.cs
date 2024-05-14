using MyRecipeBook.Communication.Enum;

namespace MyRecipeBook.Communication.Responses;

public class ResponseRecipeJson
{
    public string Title { get; set; }
    public Category Category { get; set; }
    public string MethodPrepar { get; set; }
    public double TimePreparationMinutes { get; set; }
    public List<ResponseIngredientsJson> Ingredients { get; set; }
    public Guid RecipeIdentifier { get; set; }
}
