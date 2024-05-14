namespace MyRecipeBook.Domain.Repositories.User;

public interface IUserReadOnlyReapository
{
    public Task<bool> ExistsActiveUserEmail(string email);

    public Task<Entities.User?> GetByEmailAndPassword(string email, string password);

    public Task<bool> ExistActiveUserWithIdentifier(Guid userIdenfier);
}
