using MyRecipeBook.Communication.Enum;

namespace MyRecipeBook.Communication.Responses;

public class ResponseIngredientsJson
{
    public Guid IngredientsIdentifier { get; set; }
    public string Name { get; set; }
    public double Quantity { get; set; }
    public UnitMeasure UnitMeasure { get; set; }
}
