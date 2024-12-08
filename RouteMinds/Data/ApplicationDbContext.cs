using Microsoft.EntityFrameworkCore;
using RouteMinds.Models;

namespace RouteMinds.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Rutas> Rutas { get; set; }
        public DbSet<Tiendas> Tiendas { get; set; }
        public DbSet<Almacenes> Almacenes { get; set; }
    }
}
