using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Infraestructure.DataAccess;

//Informando que essa classa vai gerenciar o DB CONTEXT -> Intermediário entre o banco de dados e a aplicação
public class MyRecipeBookDbContext : DbContext
{
    public MyRecipeBookDbContext(DbContextOptions options) : base(options) { } //-> Vai passar essa opções para o construtor do DB Context

    public DbSet<User> users { get; set; } //Informando que existe a tabela GetUser e esse users é a propia tabela para ser acessada 

    public DbSet<Recipe> recipes { get; set; }

    public DbSet<Ingredients> ingredients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Informando ao Entity que ele precisa usar as configurações dessa classem (no caso o propio assembly)
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyRecipeBookDbContext).Assembly);
    }
}
