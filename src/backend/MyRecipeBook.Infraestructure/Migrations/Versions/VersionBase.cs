using FluentMigrator;
using FluentMigrator.Builders.Create.Table;

namespace MyRecipeBook.Infraestructure.Migrations.Versions;

//Passo a responsabilidade da função UP para quem impementar a VersionBase, por ser abstract como a ForwardOnlyMigration
public abstract class VersionBase : ForwardOnlyMigration
{
    //Garantindo que apenas classes que herdem de VersionBase acessessem esse metodo
    protected ICreateTableColumnOptionOrWithColumnSyntax CreateTable(string table)
    {
        return Create.Table(table)
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("CreatedOn").AsDateTime().NotNullable()
            .WithColumn("Active").AsBoolean().NotNullable();
    }
}
