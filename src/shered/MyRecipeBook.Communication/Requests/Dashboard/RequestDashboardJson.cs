using MyRecipeBook.Communication.Enum;

namespace MyRecipeBook.Communication.Requests.Dashboard;

public class RequestDashboardJson
{
    public string TitleOrIngredientName { get; set; } = string.Empty;
    public double? MaxTime { get; set; } = null;
    public Category? Category { get; set; }
}
