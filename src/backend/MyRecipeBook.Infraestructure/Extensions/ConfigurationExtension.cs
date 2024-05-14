using Microsoft.Extensions.Configuration;
using MyRecipeBook.Domain.Enum;

namespace MyRecipeBook.Infraestructure.Extensions;

/// <summary>
/// Precisa ser static pra ser uma extension
/// </summary>
public static class ConfigurationExtension
{
    public static bool IsUnitTestEnvioriment(this IConfiguration configuration)
    {
        var testEnvioriment = configuration.GetSection("InMemoryDbTest").Value;
        return bool.TryParse(testEnvioriment, out _);
    }

    public static DataBaseType DataBaseType(this IConfiguration configuration)
    {
        var dataBaseType = configuration.GetConnectionString("dataBaseType");

        return (DataBaseType)Enum.Parse(typeof(DataBaseType), dataBaseType);
    }

    public static string ConnectionString(this IConfiguration configuration)
    {
        var dataBaseType = configuration.DataBaseType();

        if(dataBaseType == MyRecipeBook.Domain.Enum.DataBaseType.MySql)
            return configuration.GetConnectionString("ConnectionMySQLServer")!; //-> Garantindo que nunca será null
        else
            return configuration.GetConnectionString("ConnectionSQLServer")!; 
    }
}
