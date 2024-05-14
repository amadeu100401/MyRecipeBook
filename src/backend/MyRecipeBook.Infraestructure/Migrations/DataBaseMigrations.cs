using Dapper;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Enum;
using MySqlConnector;
using System.Data.SqlClient;

namespace MyRecipeBook.Infraestructure.Migrations;

public static class DataBaseMigrations
{
    public static void Migrate(DataBaseType dataBaseType, string connectionString, IServiceProvider serviceProvider)
    {
        if(dataBaseType == DataBaseType.MySql)
            EnsureDataBaseCreated_MySql(connectionString);
        else
            EnsureDataBaseCreated_SqlServer(connectionString);

        MigrationDataBse(serviceProvider);
    }

    private static void EnsureDataBaseCreated_MySql(string connectionString)
    {
        var connectionStringBuilder = new MySqlConnectionStringBuilder(connectionString);
        var dataBaseName = connectionStringBuilder.Database;

        //Removendo o nome para a conexão ocorrer apenas no servidor e não no db
        connectionStringBuilder.Remove("DataBase");

        //Criando a conexão com o banco de dados
        using var dbConnection = new MySqlConnection(connectionStringBuilder.ConnectionString);

        //Passando o db name como paratro dinamico para a query
        var parameters = new DynamicParameters();
        parameters.Add("dbName", dataBaseName);

        var records = dbConnection.Query("SELECT * FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = @dbName", parameters);

        if (!records.Any())
            dbConnection.Execute($"CREATE DATABASE {dataBaseName};");
    }

    private static void EnsureDataBaseCreated_SqlServer(string connectionString)
    {
        var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
        var dataBaseName = connectionStringBuilder.InitialCatalog;

        //Removendo o nome para a conexão ocorrer apenas no servidor e não no db
        connectionStringBuilder.Remove("DataBase");

        //Criando a conexão com o banco de dados
        using var dbConnection = new SqlConnection(connectionStringBuilder.ConnectionString);

        //Passando o db name como paratro dinamico para a query
        var parameters = new DynamicParameters();
        parameters.Add("dbName", dataBaseName);

        var records = dbConnection.Query("SELECT * FROM SYS.DATABASES WHERE NAME = @dbName");

        if (!records.Any())
            dbConnection.Execute($"CREATE DATABSE {dataBaseName};");
    }

    //Services provider é o serviço da inejção de dependencia || Aplicando as migrations no banco de dados
    private static void MigrationDataBse(IServiceProvider serviceProvider)
    {
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

        runner.ListMigrations();

        runner.MigrateUp(); 
    }
}
