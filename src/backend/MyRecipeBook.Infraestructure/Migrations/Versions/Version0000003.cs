using FluentMigrator;
using System.Data;

namespace MyRecipeBook.Infraestructure.Migrations.Versions;

[Migration(DataBaseVersions.UPDATE_USER_RECIPE, "Update the foreign key column")]
public class Version0000003 : VersionBase
{
    public override void Up()
    {
        // Remover as chaves estrangeiras existentes
        Delete.ForeignKey("FK_Ingredients_Recipe_Id").OnTable("ingredients");
        Delete.ForeignKey("FK_Recipe_User_Id").OnTable("recipes");

        Create.ForeignKey("FK_Ingredients_Recipe_Id")
            .FromTable("ingredients").ForeignColumn("RecipeId")
            .ToTable("recipes").PrimaryColumn("Id")
            .OnDelete(Rule.Cascade);

        Create.ForeignKey("FK_Recipe_User_Id")
            .FromTable("recipes").ForeignColumn("userId")
            .ToTable("users").PrimaryColumn("Id")
            .OnDelete(Rule.Cascade);
    }
}
