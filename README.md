# Projeto MyRecipeBook Backend em .NET com MySQL

Este é um projeto de backend desenvolvido em .NET que utiliza MySQL como banco de dados.

## Requisitos

Certifique-se de ter os seguintes requisitos instalados em seu sistema:

- .NET SDK 
- MySQL Server 
- Visual Studio ou Visual Studio Code (opcional)

## Configuração do Banco de Dados

1. Instale e configure o MySQL Server em sua máquina.
2. Crie um novo banco de dados para o projeto.
3. Configure as credenciais de acesso ao banco de dados no arquivo `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=NomeDoBancoDeDados;Uid=Usuario;Pwd=Senha;"
  }
}
```

Substitua NomeDoBancoDeDados, Usuario e Senha pelas informações corretas.

Executando o Projeto
Clone este repositório em sua máquina.
Abra o projeto em seu ambiente de desenvolvimento (Visual Studio ou Visual Studio Code).
Restaure as dependências do projeto utilizando o comando:

dotnet restore

Execute as migrações do banco de dados para criar as tabelas necessárias:
dotnet ef database update

Execute o projeto utilizando o comando:
dotnet run

O servidor estará sendo executado em http://localhost:5000.

Contribuindo
Sinta-se à vontade para contribuir com melhorias, correções de bugs ou novas funcionalidades. Basta abrir uma issue ou enviar um pull request.

Licença
Este projeto está licenciado sob a MIT License.

Basta copiar e colar esse conteúdo em um arquivo Markdown (.md) e adaptar conforme necessário.

