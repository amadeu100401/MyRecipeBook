using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.User;

namespace MyRecipeBook.Infraestructure.DataAccess.Repositories;

public class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyReapository, IUserUpdateOnlyRepository
{
    private readonly MyRecipeBookDbContext _dbContext;

    //Construtor da classe
    public UserRepository(MyRecipeBookDbContext dbContext) => _dbContext = dbContext;

    //Dar preferência ao async no banco de dados (performace)
    public async Task AddUser (User user) => await _dbContext.users.AddAsync(user);

    public async Task<bool> ExistsActiveUserEmail(string email)
    {
        return await _dbContext.users.AnyAsync(user => user.Email.Equals(email) && user.Active);
    }

    public async Task<bool> ExistActiveUserWithIdentifier(Guid userIdentifier) => await _dbContext.users.AnyAsync(user => user.UserIdentifier.Equals(userIdentifier) && user.Active);

    public async Task<User> GetByEmailAndPassword(string email, string password)
    {
        //AsNoTracking -> porque não vai ser uma entidade que sofrerá update no banco de dados (performance melhor)
        return await _dbContext.users.AsNoTracking()
            .FirstOrDefaultAsync(user => user.Active && user.Email.Equals(email) && user.Password.Equals(password));
    }

    public async Task<User> GetById(long id)
    {
        return await _dbContext.users.FirstAsync(user => user.Active && user.Id == id);
    }

    public void Update(User user) => _dbContext.users.Update(user);
}
