using Moq;
using MyRecipeBook.Domain.Repositories;

namespace CommonTestUtilities.Repositories;

public class UnityOfWorkBuilder
{ //Como a função commit do unityOfWork não devolve nada, vc não precisa fazer uma config, apenas devolver a implemtação

    public static IUnitOfWork Build()
    {
        //Vamos utilizar o MOQ -> Passamos a interface e ele devolve uma implementação FAKE da interface (Melhor so usar com interfaces)
        var mock = new Mock<IUnitOfWork>();

        //Essa é a implementação FAKE 
        return mock.Object; 
    }
}
