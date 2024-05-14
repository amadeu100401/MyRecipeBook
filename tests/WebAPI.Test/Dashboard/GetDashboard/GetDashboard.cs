#region USING
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using FluentAssertions;
using MyRecipeBook.Domain.Enum;
using System.Net;
using System.Text.Json;
#endregion

namespace WebAPI.Test.Dashboard.GetDashboard;

public class GetDashboard : MyRecipeBookClassFixture
{
    private readonly string METHOD = "dashboard";
    private readonly Guid _userIdentifier;
    private readonly string _recipeTitle;
    private readonly Category _recipeCategory;
    private readonly double _recipeMaxTime;
    private readonly long _userId; 

    public GetDashboard(CustomWebApplicationFactory factory) : base(factory)
    {
        _userIdentifier = factory.GetUserIdentifier();
        _recipeCategory = factory.GetRecipeCategory();
        _recipeTitle = factory.GetRecipeTitle();
        _recipeMaxTime = factory.GetRecipeTimePreparation();
        _userId = factory.GetUserId();
    }

    [Fact]
    public async Task Success_Without_Parameters()
    {
        var request = RequestDashboardJsonBuilder.Build();

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoGet(METHOD, token);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData= await JsonDocument.ParseAsync(responseBody);

        var recipeList = responseData.RootElement.GetProperty("usersRecipes").EnumerateArray();

        recipeList.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Success_Title_Recipes()
    {
        var request = RequestDashboardJsonBuilder.Build(title: _recipeTitle);

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var parametersDictionary = CreateParametersDictionary(recipeTitle: request.TitleOrIngredientName);

        var response = await DoGet(METHOD, token, parameters: parametersDictionary);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var recipeList = responseData.RootElement.GetProperty("usersRecipes").EnumerateArray();

        foreach(JsonElement recipe in recipeList)
        {
            var recipeTitle = recipe.GetProperty("title").GetString();
            recipeTitle.Should().NotBeNullOrWhiteSpace().And.Be(request.TitleOrIngredientName);
        }
    }

    [Fact]
    public async Task Success_MaxTime_Recipes()
    {
        var request = RequestDashboardJsonBuilder.Build(maxTime: _recipeMaxTime);

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var parameters = CreateParametersDictionary(maxTime: request.MaxTime);

        var response = await DoGet(METHOD, token, parameters: parameters);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var recipeList = responseData.RootElement.GetProperty("usersRecipes").EnumerateArray();

        foreach (JsonElement recipe in recipeList)
        {
            var recipeTime = recipe.GetProperty("timePreparationMinutes").GetDouble();

            recipeTime.Should().BeLessThanOrEqualTo((double)request.MaxTime!);
        }
    }

    [Fact]
    public async Task Success_Category_Recipes()
    {
        var request = RequestDashboardJsonBuilder.Build(category: (MyRecipeBook.Communication.Enum.Category)_recipeCategory);

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var parameters = CreateParametersDictionary(category: (MyRecipeBook.Domain.Enum.Category)request.Category!);

        var response = await DoGet(METHOD, token, parameters: parameters);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var recipeList = responseData.RootElement.GetProperty("usersRecipes").EnumerateArray();

        recipeList.Should().HaveCountGreaterThanOrEqualTo(1);

        foreach (JsonElement recipe in recipeList)
        {
            var recipeCategory = Int32.Parse(recipe.GetProperty("category").ToString());

            bool isValid = Enum.IsDefined(typeof(MyRecipeBook.Communication.Enum.Category), recipeCategory) &&
                ((MyRecipeBook.Communication.Enum.Category)recipeCategory == request.Category);

            isValid.Should().BeTrue();
        }
    }

    private IDictionary<string, string> CreateParametersDictionary(string recipeTitle = null, double? maxTime = null, Category? category = null)
    {
        Dictionary<string, string> parameters = new Dictionary<string, string>();

        if(!string.IsNullOrWhiteSpace(recipeTitle))
        {
            parameters.Add("TitleOrIngredientName", value: recipeTitle);
        }
        else if(maxTime != null)
        {
            parameters.Add("MaxTime", value: maxTime.ToString()!);
        }
        else if(category != null)
        {
            parameters.Add("Category", value: category.ToString()!);
        }

        return parameters;
    }
}
