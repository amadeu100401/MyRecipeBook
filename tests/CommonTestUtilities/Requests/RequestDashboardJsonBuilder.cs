using MyRecipeBook.Communication.Requests.Dashboard;

namespace CommonTestUtilities.Requests;

public class RequestDashboardJsonBuilder
{
    public static RequestDashboardJson Build(string title = "", MyRecipeBook.Communication.Enum.Category? category = null, double? maxTime = null)
    {
        return new RequestDashboardJson 
        {
            TitleOrIngredientName = title,
            Category = category,
            MaxTime = maxTime
        };
    }
}
