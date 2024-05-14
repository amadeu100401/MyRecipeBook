using MyRecipeBook.Communication.Requests.Dashboard;
using MyRecipeBook.Communication.Responses.Dashboard;

namespace MyRecipeBook.Application.UseCases.Dashboard;

public interface IGetDashboardUseCase
{
    public Task<ResponseDashboardJson> Execute(RequestDashboardJson request);
}
