using FluentMigrator;

namespace MyRecipeBook.Infraestructure.Migrations.Versions;

[Migration(DataBaseVersions.TABLE_RECIPE, "Create table to save recipe's information")]
public class Version0000002 : VersionBase
{
    public override void Up()
    {
        CreateTable("recipes")
            .WithColumn("Title").AsAnsiString(100).NotNullable()
            .WithColumn("Category").AsInt16().NotNullable()
            .WithColumn("MethodPrepar").AsAnsiString().NotNullable()
            .WithColumn("TimePreparationMinutes").AsDouble().NotNullable()
            .WithColumn("RecipeIdentifier").AsAnsiString().Unique().NotNullable()
            .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("FK_Recipe_User_Id", "users", "Id");

        CreateTable("ingredients")
            .WithColumn("Name").AsAnsiString().NotNullable()
            .WithColumn("Quantity").AsDouble().NotNullable()
            .WithColumn("UnitMeasure").AsInt16().NotNullable()
            .WithColumn("IngredientsIdentifier").AsAnsiString().Unique().NotNullable()
            .WithColumn("RecipeId").AsInt64().NotNullable().ForeignKey("FK_Ingredients_Recipe_Id", "recipes", "Id") ;
    }
}
