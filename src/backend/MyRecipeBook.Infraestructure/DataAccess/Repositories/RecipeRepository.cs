using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.Recipe;

namespace MyRecipeBook.Infraestructure.DataAccess.Repositories;

public class RecipeRepository : IRecipeWriteOnlyRepository, IRecipeReadOnlyRepository, IRecipeUpdateOnlyRepository, IRecipeDeleteOnlyRepository
{
    private readonly MyRecipeBookDbContext _dbContext;

    public RecipeRepository(MyRecipeBookDbContext dbContext) => _dbContext = dbContext;

    public async Task AddRecipe(Recipe recipe) => await _dbContext.recipes.AddAsync(recipe);

    public async Task<bool> ExistsRecipe(long userId, Guid recipeIdentifier)
    {
        return await _dbContext.recipes.AnyAsync(recipe => recipe.UserId == userId
            && recipe.RecipeIdentifier.Equals(recipeIdentifier));
    }

    public async Task<List<Recipe>> GetRecipe(long userId)
    {
        return await _dbContext.recipes.AsNoTracking()
            .Include(recipe => recipe.Ingredients)
            .Where(recipe => recipe.UserId == userId).ToListAsync(); ;
    }

    public async Task Update(Recipe recipe)
    {
        var existeRecipe = await _dbContext.recipes.Include(recipe => recipe.Ingredients)
            .FirstOrDefaultAsync(recipe => recipe.RecipeIdentifier == recipe.RecipeIdentifier);

        if (existeRecipe != null)
        {
            existeRecipe.Title = recipe.Title;
            existeRecipe.Category = recipe.Category;
            existeRecipe.TimePreparationMinutes = recipe.TimePreparationMinutes;
            existeRecipe.MethodPrepar = recipe.MethodPrepar;

            foreach (var existingIngredient in existeRecipe.Ingredients)
            {
                var newIngredient = recipe.Ingredients.FirstOrDefault(i => i.Id == existingIngredient.Id);

                if (newIngredient != null)
                {
                    existingIngredient.Name = newIngredient.Name;
                    existingIngredient.Quantity = newIngredient.Quantity;
                    existingIngredient.UnitMeasure = newIngredient.UnitMeasure;
                }
            }
        } 
    }

    public async Task Delete(long userId , Guid recipeIdentifier) 
    {
        var recipe = _dbContext.recipes.Where(recipe => recipe.UserId == userId && recipe.RecipeIdentifier == recipeIdentifier).ToList();

        if (recipe != null)
            _dbContext.recipes.RemoveRange(recipe);
    }
}
