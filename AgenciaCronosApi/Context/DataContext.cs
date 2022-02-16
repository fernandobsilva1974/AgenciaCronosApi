using Microsoft.EntityFrameworkCore;
using AgenciaCronosApi.Models;

namespace AgenciaCronosApi.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Servicos> Servicos { get; set; }

        public DbSet<Posts> Posts { get; set; }

        public DbSet<Integrantes> Integrantes { get; set; }
    }
}
