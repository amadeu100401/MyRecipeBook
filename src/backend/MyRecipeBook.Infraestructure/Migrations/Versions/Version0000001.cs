using FluentMigrator;

namespace MyRecipeBook.Infraestructure.Migrations.Versions;

[Migration(DataBaseVersions.TABLE_USER, "Create table to save user's information")]
public class Version0000001 : VersionBase
{
    public override void Up()
    {
        CreateTable("users")
            .WithColumn("Name").AsAnsiString(150).NotNullable()
            .WithColumn("Email").AsAnsiString(255).NotNullable()
            .WithColumn("Password").AsAnsiString(2000).NotNullable()
            .WithColumn("UserIdentifier").AsGuid().NotNullable();
    }
}
