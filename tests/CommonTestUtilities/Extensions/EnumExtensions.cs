namespace CommonTestUtilities.Extensions;

public static class EnumExtensions
{
    public static T GetRandomEnumValue<T>(this T numType) where T : Enum
    {
        var values = Enum.GetValues(typeof(T));
        var random = new Random();
        return (T)values.GetValue(random.Next(values.Length));
    }
}
