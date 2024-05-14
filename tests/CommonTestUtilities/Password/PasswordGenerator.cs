using System.Text;

namespace CommonTestUtilities.Password;

public class PasswordGenerator
{
    private static Random random = new Random();

    public static string GeneratePassword(int length)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        StringBuilder password = new StringBuilder();

        // Adiciona pelo menos uma letra maiúscula e um número à senha
        password.Append(chars[random.Next(26, 52)]); // Uma letra maiúscula
        password.Append(chars[random.Next(52, 62)]); // Um número

        // Preenche o restante da senha com caracteres aleatórios
        for (int i = 2; i < length; i++)
        {
            password.Append(chars[random.Next(chars.Length)]);
        }

        // Embaralha a senha para torná-la mais aleatória
        return Shuffle(password.ToString());
    }

    private static string Shuffle(string str)
    {
        char[] array = str.ToCharArray();
        int n = array.Length;

        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            char value = array[k];
            array[k] = array[n];
            array[n] = value;
        }

        return new string(array);
    }
}
