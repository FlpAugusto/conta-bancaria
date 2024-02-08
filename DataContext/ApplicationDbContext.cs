using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.DataContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ContaBancaria> ContaBancaria { get; set; }
        public DbSet<Transacoes> Transacoes { get; set; }
    }
}
