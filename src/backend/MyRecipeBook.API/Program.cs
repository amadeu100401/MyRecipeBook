#region USING
using Microsoft.OpenApi.Models;
using MyRecipeBook.API.Converters;
using MyRecipeBook.API.Filters;
using MyRecipeBook.API.Middleware;
using MyRecipeBook.API.Token;
using MyRecipeBook.Application;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Infraestructure;
using MyRecipeBook.Infraestructure.Extensions;
using MyRecipeBook.Infraestructure.Migrations;
#endregion

var builder = WebApplication.CreateBuilder(args);

// Add services to the container. -> Passando o stringConverter para ser chamado
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new StringConverter()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    //Gerando a definição 
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        //Isso vai gerar uma opção pra inserir um JWT Header no Swagger
        Description = @"JWT Authorization header using Bearer scheme.
                    Enter 'Bearer' [space] and then your token in the text input below.
                    Exemple: 'Bearer12345abcdef' ",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    //Gerando o requerimento 
    options.AddSecurityRequirement(new OpenApiSecurityRequirement 
    {
        {
            new OpenApiSecurityScheme
            {
              Reference = new OpenApiReference
              {
                  Type = ReferenceType.SecurityScheme,
                  Id = "Bearer"
              },
              Scheme = "oauth2",
              Name =  "Bearer",
              In = ParameterLocation.Header
            },
            new List<string>()
        }
    });

}); //Gerando o swagger

//Fazendo com que o filtro de excecoes seja enxergado pela API e que essa classe precisa ser lançada
builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));

//Fazendo a injeção de dependência 
builder.Services.AddInfraestruture(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddScoped<ITokenProvider, HttpTokenValue>( );

//Configurando as URLs para serem minusculas
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Configurando a classe como middleware
app.UseMiddleware<CultureMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

MigrateDataBase();

app.Run();

void MigrateDataBase()
{
    if (builder.Configuration.IsUnitTestEnvioriment())
        return;

    var dataBaseType = builder.Configuration.DataBaseType();
    var connectionString = builder.Configuration.ConnectionString();

    //Criando um service scope para poder pegar o serviceProvide
    var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope(); 

    DataBaseMigrations.Migrate(dataBaseType, connectionString, serviceScope.ServiceProvider);
}

//Isso serve pra que o servidor de teste consiga chamar essa aplicação pra rodar no servidor como EntryPoint
public partial class Program
{
    protected Program() { }
}