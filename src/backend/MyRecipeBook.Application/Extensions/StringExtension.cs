using System.Globalization;
using System.Text;

namespace MyRecipeBook.Application.Extensions;

public static class StringExtension
{
    public static bool CompareStrings(this string principalWord, string wordToCompare)
    {
        var normalizedPrincipalWord = RemoveAccents(principalWord).ToLower();
        var normalizedWord = RemoveAccents(wordToCompare).ToLower();

        return normalizedPrincipalWord.Contains(normalizedWord);

    }

    public static string RemoveAccents(this string input)
    {
        // Normaliza a string para a forma de decomposição
        string normalizedString = input.Normalize(NormalizationForm.FormD);

        // Cria um StringBuilder para armazenar os caracteres não acentuados
        StringBuilder stringBuilder = new StringBuilder();

        // Itera sobre cada caractere na string normalizada
        foreach (char c in normalizedString)
        {
            // Verifica se o caractere não é uma marca diacrítica
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c); // Adiciona o caractere ao StringBuilder
            }
        }

        // Retorna a string resultante sem acentos
        return stringBuilder.ToString();
    }
}
