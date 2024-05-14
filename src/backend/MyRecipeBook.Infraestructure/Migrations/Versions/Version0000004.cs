using FluentMigrator;

namespace MyRecipeBook.Infraestructure.Migrations.Versions;

[Migration(DataBaseVersions.UPDATE_METHOD_COLUMN, "Increasing the character limit for the preparation method.")]
public class Version0000004 : VersionBase
{
    public override void Up()
    {
        Alter.Column("MethodPrepar").OnTable("recipes").AsString(2000).NotNullable();   
    }
}
