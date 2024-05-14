using AutoMapper;
using MyRecipeBook.Communication.Requests.RecipeRequest;
using MyRecipeBook.Communication.Requests.UserRquest;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Application.Services.AutoMapper;

/*--Essa classe vai definir as configurações para o auto mapper
Requisicao => Entidade e Entidade => Response --*/
public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToDomain();
        DomainToResponse();
    }

    private void RequestToDomain()
    {
        //De onde está vindo os dados e para qual entidade eles vão
        CreateMap<RequestRegisterUserJson, User>()
            .ForMember(dest => dest.Password, opt => opt.Ignore());

        CreateMap<RequestRegisterRecipe, Recipe>()
            .ForMember(dest => dest.RecipeIdentifier, opt => opt.Ignore());

        CreateMap<RequestRegisterIngredient, Ingredients>()
            .ForMember(dest => dest.IngredientsIdentifier, opt => opt.Ignore());
    }

    private void DomainToResponse()
    {
        CreateMap<User, ResponseUserProfile>();
        CreateMap<Recipe, ResponseRecipeJson>();
        CreateMap<Ingredients, ResponseIngredientsJson>();
    }
}
