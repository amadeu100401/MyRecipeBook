#region USING
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Application.Services.AutoMapper;
using MyRecipeBook.Application.UseCases.Dashboard;
using MyRecipeBook.Application.UseCases.Recipe.CreateRecipe;
using MyRecipeBook.Application.UseCases.Recipe.DeleteRecipe;
using MyRecipeBook.Application.UseCases.Recipe.GetCategory;
using MyRecipeBook.Application.UseCases.Recipe.GetRecipe;
using MyRecipeBook.Application.UseCases.Recipe.GetUnitMeasure;
using MyRecipeBook.Application.UseCases.Recipe.UpdateRecipe;
using MyRecipeBook.Application.UseCases.User.ChangePassword;
using MyRecipeBook.Application.UseCases.User.Desactivate;
using MyRecipeBook.Application.UseCases.User.Login.DoLogin;
using MyRecipeBook.Application.UseCases.User.Profil;
using MyRecipeBook.Application.UseCases.User.Profile;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Application.UseCases.User.Update;
#endregion

namespace MyRecipeBook.Application;

//Esse classe serve pra que seja posível fazer a inejção de dependencia dentro do projeto da API
public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    { 
        AddAutoMapper(services);
        AddUseCases(services);
    }

    private static void AddAutoMapper(IServiceCollection services)
    {
        var autoMapper = new AutoMapper.MapperConfiguration(options =>
        {
            options.AddProfile(new AutoMapping());
        }).CreateMapper();

        services.AddScoped(options => autoMapper);
    }

    private static void AddUseCases(IServiceCollection services)
    {
        //Sempre que alguem precisar dessa IUseCase, será retornado a instância de RegisterUserCase
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
        services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
        services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();
        services.AddScoped<IDesactivateUserUseCase, DesactivateUserUseCase>();

        //Recipes
        services.AddScoped<ICreateRecipeUseCase, CreateRecipeUseCase>();
        services.AddScoped<IGetRecipeUseCase, GetRecipeUseCase>();
        services.AddScoped<IUpdateRecipeUseCase, UpdateRecipeUseCase>();
        services.AddScoped<IDeleteRecipeUseCase, DeleteRecipeUseCase>();
        services.AddScoped<IGetCategoryUseCase, GetCategoryUseCase>();
        services.AddScoped<IGetUnitMeasureUseCase, GetUnitMeasureUseCase>();

        //Dashboard
        services.AddScoped<IGetDashboardUseCase, GetDashboardUseCase>();
    }
}
