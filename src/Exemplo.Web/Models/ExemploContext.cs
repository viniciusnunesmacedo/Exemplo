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