using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Infraestructure.DataAccess;

namespace WebAPI.Test;

//Precisamos informar pra o .NET que essa classe é um teste de integração 
//Cada classe vai rodar em um servidor (as classes não os métodos)
public class CustomWebApplicationFactory : WebApplicationFactory<Program> //Servidor fornecido pela propia Microsoft || Farei configurações propias para usar o db in memory
{
    private MyRecipeBook.Domain.Entities.User _user = default!; //Garantindo para o .NET que essa sintax n será nula
    private string _password = string.Empty;
    private string _name = string.Empty;
    private long _userId;

    private MyRecipeBook.Domain.Entities.Recipe _recipe = default!;
    private string _recipeTitle = string.Empty;
    private MyRecipeBook.Domain.Enum.Category _recipeCategory = default!;
    private double _recipeTimePreparation = default!;
    private Guid _recipeIdentifier;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        //Definindo ambiente Test
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                //Passando o DbContext da aplicação para verificar se esse DbContext Já existe
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<MyRecipeBookDbContext>));

                //Caso o DbContext já tenha sido adicionado é preciso remove-lo
                //Para não acessar o banco de dados real
                if(descriptor is not null)
                {
                    services.Remove(descriptor);
                }

                //Configurando o sistema pra usar o banco de dados em memoria - Criando o banco de dados em momoria
                var provide = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                //Colocando o banco local criado para ser usado no DbContext vazio
                services.AddDbContext<MyRecipeBookDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(provide);
                });

                using var scope = services.BuildServiceProvider().CreateScope();

                var dbContext = scope.ServiceProvider.GetRequiredService<MyRecipeBookDbContext>();

                //Garantindo que a base vai começar vazia
                dbContext.Database.EnsureDeleted();

                StartDataBase(dbContext);
             });
    }

    public string GetEmail()
    {
        return _user.Email;
    }

    public string GetPassword()
    {
        return _password;
    }

    public string GetName()
    {
        _name = _user.Name;
        return _name;
    }

    public long GetUserId()
    {
        _userId = _user.Id;
        return _userId;
    }

    public Guid GetUserIdentifier() => _user.UserIdentifier;

    public string GetRecipeTitle()
    {
        _recipeTitle = _recipe.Title;
        return _recipeTitle;
    }
    
    public MyRecipeBook.Domain.Enum.Category GetRecipeCategory()
    {
        _recipeCategory = _recipe.Category;
        return _recipeCategory;
    }

    public double GetRecipeTimePreparation()
    {
        _recipeTimePreparation = _recipe.TimePreparationMinutes;
        return _recipeTimePreparation;
    }

    public Guid GetRecipeIdentifier()
    {
        _recipeIdentifier = _recipe.RecipeIdentifier;
        return _recipeIdentifier;
    }

    private void StartDataBase(MyRecipeBookDbContext dbContext)
    {
        (_user, _password) = UserBuilder.Build();

        _recipe = RecipeBuilder.Build();
        _recipe.UserId = _user.Id;

        dbContext.users.Add(_user);
        dbContext.recipes.Add(_recipe);

        dbContext.SaveChanges();
    }
}
