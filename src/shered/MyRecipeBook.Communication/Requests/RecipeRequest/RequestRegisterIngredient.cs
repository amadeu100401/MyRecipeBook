using MyRecipeBook.Communication.Enum;

namespace MyRecipeBook.Communication.Requests.RecipeRequest;

public class RequestRegisterIngredient
{
    public string Name { get; set; }
    public double? Quantity { get; set; }
    public UnitMeasure UnitMeasure { get; set; }
}
