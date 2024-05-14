namespace MyRecipeBook.Application.Extensions;

public static class EnumExtensions
{
    public static bool IsEqualTo<T>(this T value, Communication.Enum.Category category, T compareTo) where T : struct, Enum
    {
        return value.Equals(compareTo);
    }

}
