using MyRecipeBook.Communication.Enum;

namespace MyRecipeBook.Communication.Requests.RecipeRequest;

public class RequestRegisterRecipe
{
    public RequestRegisterRecipe()
    {
        Ingredients = new();
    }
    public string Title { get; set; }
    public Category Category { get; set; }
    public string MethodPrepar { get; set; }
    public double TimePreparationMinutes { get; set; }
    public List<RequestRegisterIngredient> Ingredients { get; set; }
}
