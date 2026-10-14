using Microsoft.EntityFrameworkCore;
using Pedidos.Domain;

namespace Pedidos.Infrastructure;

public class PedidosDbContext(DbContextOptions<PedidosDbContext> options) : DbContext(options)
{
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>().HasIndex(x => x.Email).IsUnique();
        modelBuilder.Entity<Producto>().Property(x => x.Precio).HasPrecision(12, 2);
        modelBuilder.Entity<Pedido>().HasOne(x => x.Cliente).WithMany(x => x.Pedidos).HasForeignKey(x => x.ClienteId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Pedido>().HasOne(x => x.Producto).WithMany(x => x.Pedidos).HasForeignKey(x => x.ProductoId).OnDelete(DeleteBehavior.Restrict);
    }
}
