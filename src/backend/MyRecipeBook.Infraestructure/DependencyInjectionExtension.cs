#region Using
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Enum;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Infraestructure.DataAccess;
using MyRecipeBook.Infraestructure.DataAccess.Repositories;
using MyRecipeBook.Infraestructure.Extensions;
using MyRecipeBook.Infraestructure.Security.Cryptography;
using MyRecipeBook.Infraestructure.Security.Tokens.Access.Generator;
using MyRecipeBook.Infraestructure.Security.Tokens.Access.Validator;
using MyRecipeBook.Infraestructure.Services.LoggedUser;
using System.Reflection;
#endregion

namespace MyRecipeBook.Infraestructure;

//Esse classe serve pra que seja posível fazer a inejção de dependencia dentro do projeto da API
//Assim que essa di for feita, tudo iriá por osmose
public static class DependencyInjectionExtension
{
    //o this é porque essa função é uma extension
    public static void AddInfraestruture(this IServiceCollection services, IConfiguration configuration)
    {
        //Injeção de dependencia pro repositorio
        AddRepositories(services);
        AddToken(services, configuration);
        AddLoggedUser(services);
        AddPasswordEncripter(services, configuration);

        if (configuration.IsUnitTestEnvioriment())
            return;

        var dataBaseType = configuration.DataBaseType();

        if (dataBaseType == DataBaseType.MySql)
        {
            AddDbContext_MySql(services, configuration);
            AddFluentMigrator_MySql(services, configuration);
        }
        else
        {
            AddDbContext_SqlServer(services, configuration);
            AddFluentMigrator_SqlServer(services, configuration);
        }
    }

    private static void AddDbContext_MySql(IServiceCollection services, IConfiguration configuration)
    {
        var connectionSrtring = configuration.ConnectionString();
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 35));

        services.AddDbContext<MyRecipeBook.Infraestructure.DataAccess.MyRecipeBookDbContext>(dbContextOptions =>
        {
            dbContextOptions.UseMySql(connectionSrtring, serverVersion);
        });
    }

    //configuração para SQL Server
    private static void AddDbContext_SqlServer(IServiceCollection services, IConfiguration configuration)
    {
        var connectionSrtring = configuration.ConnectionString();

        //Precisa instalar o SQL Server Entity framework core -- não vou instalar, mas caso for usar, fazer essa alteração 
        //services.AddDbContext<MyRecipeBook.Infraestructure.DataAccess.MyRecipeBookDbContext>(dbContextOptions =>
        //{
        //    dbContextOptions.UseSqlServer(connectionSrtring);
        //});
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        //Toda vez que IUserWriteOnlyRepository for solicitado, é necessário devolver uma instancia de UserRepository
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();

        //Toda vez que IUReadOnlyRepository for solicitado, é necessário devolver uma instancia de UserRepository
        services.AddScoped<IUserReadOnlyReapository, UserRepository>();
        services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();

        //Recipe repository
        services.AddScoped<IRecipeWriteOnlyRepository, RecipeRepository>();
        services.AddScoped<IRecipeReadOnlyRepository, RecipeRepository>();
        services.AddScoped<IRecipeUpdateOnlyRepository, RecipeRepository>();
        services.AddScoped<IRecipeDeleteOnlyRepository, RecipeRepository>();
    }

    //Fluent Migrator
    private static void AddFluentMigrator_MySql(IServiceCollection services, IConfiguration configuration)
    {
        //Configurando a aplicação das migrations
        services.AddFluentMigratorCore().ConfigureRunner(options =>
        {
            var connectionSrtring = configuration.ConnectionString();

            //Informando qual é o banco de dados
            options
            .AddMySql5()
            .WithGlobalConnectionString(connectionSrtring)
            .ScanIn(Assembly.Load("MyRecipeBook.Infraestructure")) //Informando onde estão os arquivos das Migrations
            .For.All();
        });
    }

    private static void AddFluentMigrator_SqlServer(IServiceCollection services, IConfiguration configuration)
    {
        services.AddFluentMigratorCore().ConfigureRunner(options =>
        {
            var connectionSrtring = configuration.ConnectionString();

            //Informando qual é o banco de dados
            options
            .AddSqlServer()
            .WithGlobalConnectionString(connectionSrtring)
            .ScanIn(Assembly.Load("MyRecipeBook.Infraestructure"))
            .For.All();
        });
    }

    private static void AddToken(IServiceCollection services, IConfiguration configuration)
    {
        var expirationTimeMinutes = uint.Parse(configuration.GetSection("Setting:Jwt:ExpirationTimeMinutes").Value);
        var signingKey = configuration.GetSection("Setting:Jwt:SigningKey").Value;

        services.AddScoped<IAccessTokenGenerator>(option => new JwtTokenGenerator(expirationTimeMinutes, signingKey));
        services.AddScoped<IAccessTokenValidator>(option => new JwtTokenValidator(signingKey));
    }

    private static void AddLoggedUser(IServiceCollection services) => services.AddScoped<ILoggedUser, LoggedUser>();

    private static void AddPasswordEncripter(IServiceCollection services, IConfiguration configuration)
    {
        var additonalKey = configuration.GetSection("Setting:Password:AdditionalKey").Value;

        //var additonalKey = configuration.GetValue<string>("Setting:Password:AdditionalKey");

        services.AddScoped<IPasswordEncripter>(options => new Shar512Encripter(additonalKey!));
    }
}
