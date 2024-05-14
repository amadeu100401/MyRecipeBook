using MyRecipeBook.Domain.Repositories;

namespace MyRecipeBook.Infraestructure.DataAccess;

public class UnitOfWork : IUnitOfWork
{
    private readonly MyRecipeBookDbContext _dbContext;

    public UnitOfWork(MyRecipeBookDbContext dbContext)
    {
        _dbContext = dbContext;
    }
     //Persiste os dados no banco de dados caso dê tudo certo
    public async Task Commit() => await _dbContext.SaveChangesAsync();
}
