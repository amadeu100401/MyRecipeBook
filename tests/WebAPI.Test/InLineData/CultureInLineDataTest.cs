using System.Collections;

namespace WebAPI.Test.InLineData;

public class CultureInLineDataTest : IEnumerable<Object[]> //esse object é o tipo base pra todos os tipos no do c#
{
    public IEnumerator<object[]> GetEnumerator()
    {
        // yield -> esse yield executa o return e continua a devolver o que tiver depois desse return -> nesse caso, vão ser devolvidos todos os returns
        yield return new object[] { "en" };
        yield return new object[] { "pt-PT" };
        yield return new object[] { "pt-BR" };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator(); 
}
