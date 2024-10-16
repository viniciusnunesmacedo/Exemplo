# Projeto Exemplo

## Introdução
Projeto exemplo em Microsoft ASP NET MVC, .net core 8 e ms-sql com docker e docker compose

Selecionar ao lado esquerdo o botão Create New Project.

![image001](https://github.com/user-attachments/assets/5bfd2346-0f70-4821-9784-f11628c3d933)

Selecionar o template Asp.Net Core Web App (Model-View-Controller) MVC, Web

![image002](https://github.com/user-attachments/assets/78a1d509-0eb2-4d5b-8a29-a0082b9ac1c9)

Após o projeto criado, crie um arquivo Cliente.cs

![image003](https://github.com/user-attachments/assets/92657434-88f3-45a3-882d-cf64fb2a9bbd)

```c#
namespace Exemplo.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }
}
```
Abrir o terminal e instalar os pacotes do Entity Framework

```shell
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Tools
```


Crie um arquivo ExemploContext.cs

Com o recurso do Copilot no Visual Studio Code, solicite a seguinte criação: "Criar arquivo de contexto entity framework"

![image004](https://github.com/user-attachments/assets/d11a5895-7ee1-4af0-87d1-9ff3d392bfec)

Após a geração, este é o resultado:

![image005](https://github.com/user-attachments/assets/9ee272d0-af1b-4204-af81-cfdacfe0e43b)

```c#
using Microsoft.EntityFrameworkCore;

namespace Exemplo.Models
{
    public class ExemploContext : DbContext
    {
        public ExemploContext(DbContextOptions<ExemploContext> options) : base(options){}

        // Add DbSet properties for your entities here
         public DbSet<Cliente> Clientes { get; set; }
    }
}
```
Crie o arquivo PrepDB.cs

Este arquivo será responsável por inserir alguns dados na tabela cliente para visualização.

![image006](https://github.com/user-attachments/assets/fe86b893-9013-4af2-a421-ad4276eda3a2)

```c#
using Microsoft.EntityFrameworkCore;

namespace Exemplo.Models
{
    public class PrepDB
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ExemploContext>();
                if (context != null)
                {
                    SeedData(context);
                }
                else
                {
                    throw new Exception("Context is null");
                }
            }
        }

        private static void SeedData(ExemploContext context)
        {
            Console.WriteLine("Aplicando migrações...");

            context.Database.Migrate();

            if (!context.Clientes.Any())
            {
                Console.WriteLine("Adicionando dados de exemplo...");

                context.Clientes.AddRange(
                    new Cliente { Nome = "Alice" },
                    new Cliente { Nome = "Bob" },
                    new Cliente { Nome = "Charlie" },
                    new Cliente { Nome = "David" },
                    new Cliente { Nome = "Eve" }
                );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("Já existem dados de exemplo.");
            }
        }
    }
}

```


Após a criação, abra o arquivo Program.cs

![image007](https://github.com/user-attachments/assets/c0233601-a1ed-45ac-a8bb-de8e4b6b83d4)

Insira o código abaixo conforme a ilustração:

```C#
var server = builder.Configuration["DBServer"] ?? "ms-sql-server";
var port = builder.Configuration["DBPort"] ?? "1433";
var user = builder.Configuration["DBUser"] ?? "SA";
var password = builder.Configuration["DBPassword"] ?? "123@Mudar";
var database = builder.Configuration["Database"] ?? "Exemplo";

builder.Services.AddDbContext<ExemploContext>(options =>
    options.UseSqlServer($"Server={server},{port};Initial Catalog={database};User ID={user};Password={password};Encrypt=False"));

...

PrepDB.PrepPopulation(app); 

```

Executar o migrations do entity framework:

```shell
dotnet ef migrations add ClienteModel
```

![image008](https://github.com/user-attachments/assets/725a31be-4cbf-4828-a6bc-0d264e0b48ab)

Criar o arquivo Dockerfile

![image009](https://github.com/user-attachments/assets/3c9ef97a-0869-42e8-bb5c-98e542e74fcd)

```Dockerfile
# Get Base Image (Full .NET Core SDK)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-exemplo
WORKDIR /app

# Copy csproj and restore
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o ./app/out

# Generate runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
EXPOSE 80
COPY --from=build-exemplo ./app/out .
ENTRYPOINT [ "dotnet","Exemplo.dll" ]
![image](https://github.com/user-attachments/assets/12f8a170-39f4-45eb-8898-5caf2beff03d)

```

Criar o aquivo docker-compose.yml

![image010](https://github.com/user-attachments/assets/21d1fd2c-66f1-448c-a86d-e00d37842f4c)

```
version: '3'
services:
  ms-sql-server:
    image: mcr.microsoft.com/mssql/server:2019-latest
    environment:
      ACCEPT_EULA: "Y"
      SA_PASSWORD: "123@Mudar"
      MSSQL_PID: Express
    ports:
      - "1433:1433"
  projeto-exemplo:
    build: .
    environment:
      DBServer: "ms-sql-server"
    ports:
      - "80:8080"
```

Editar o arquivo Controllers/HomeController.cs

Incluir no construtor o parametro do contexto.

Passar no retorno da Index a lista de Clientes.

![image011](https://github.com/user-attachments/assets/98799f2d-b180-47ca-89be-2938ddb60656)

Editar o arquivo View/Home/Index.cshtml

Incluir o modelo da listagem de clientes.

Criar um laço do nome dos clientes.

![image012](https://github.com/user-attachments/assets/939c7bcf-fe54-4fa6-ba75-d4f9eda3073b)

Para testar se o código está correto, exute o seguinte comando no terminal

```
dotnet build 
```

Caso não tenha problema de compilação, execute o docker para baixar a imagem do SQL Server para Linux:

```
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=123@Mudar' -e 'MSSQL_PID=Express' -p1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest
```

Quando executado o comando, o Docker vai baixar a imagem e iniciá-la, onde pode ser testada usando uma ferramenta de manipulação de SGBD do SQL, como SSMS ou Azure

![image013](https://github.com/user-attachments/assets/f43eb82f-c1a6-4304-be20-38f2a6e52e28)

Altere no arquivo Program.cs a configuração DBServer para localhost.

Para testar a aplicação localmente com e criando o banco SQL na imagem, rodamos o comando:

```
dotnet run
```

![image014](https://github.com/user-attachments/assets/1c1b3b73-ec3b-4a18-8943-ab8710448548)

Como informado no log, o self-hosted executou na porta 5009 exibindo a lista de nomes do banco de dados.

Altere no arquivo Program.cs a configuração DBServer para ms-sql-server.

![image015](https://github.com/user-attachments/assets/76a64b1a-9fdc-483f-82ea-ea3feda8ed2e)

Crie o container da aplicação

```
docker build -t projeto-exemplo .
```

![image016](https://github.com/user-attachments/assets/3e48fc2b-5914-46e1-bbe1-1fbbfda698a1)

Execute o comando docker compose para executar iniciar o container

```
docker composse up
```

![image017](https://github.com/user-attachments/assets/a413917c-2eda-4d50-b0d7-6e26c187e999)

Agora teste a aplicação no navegador pela porta configurada no docker compose:

![image018](https://github.com/user-attachments/assets/a43143b6-a291-44c9-81a3-0dad49125af8)
