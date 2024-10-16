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
                    new Cliente() { Nome = "Alice" },
                    new Cliente() { Nome = "Bob" },
                    new Cliente() { Nome = "Charlie" },
                    new Cliente() { Nome = "David" },
                    new Cliente() { Nome = "Eve" }
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