using Microsoft.EntityFrameworkCore;
using Pedidos.Application;
using Pedidos.Domain;

namespace Pedidos.Infrastructure;

public class ClienteService(PedidosDbContext db) : IClienteService
{
    public Task<List<Cliente>> GetAllAsync() => db.Clientes.AsNoTracking().OrderBy(x => x.Nombre).ToListAsync();
    public Task<Cliente?> GetAsync(int id) => db.Clientes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    public async Task<Cliente> CreateAsync(ClienteRequest r) { if (await db.Clientes.AnyAsync(x => x.Email == r.Email)) throw new BusinessRuleException("El correo ya está registrado."); var x = new Cliente { Nombre = r.Nombre, Apellido = r.Apellido, Email = r.Email }; db.Add(x); await db.SaveChangesAsync(); return x; }
    public async Task<bool> UpdateAsync(int id, ClienteRequest r) { var x = await db.Clientes.FindAsync(id); if (x is null) return false; if (await db.Clientes.AnyAsync(c => c.Email == r.Email && c.Id != id)) throw new BusinessRuleException("El correo ya está registrado."); x.Nombre = r.Nombre; x.Apellido = r.Apellido; x.Email = r.Email; await db.SaveChangesAsync(); return true; }
    public async Task<bool> DeleteAsync(int id) { var x = await db.Clientes.FindAsync(id); if (x is null) return false; db.Remove(x); await db.SaveChangesAsync(); return true; }
}

public class ProductoService(PedidosDbContext db) : IProductoService
{
    public Task<List<Producto>> GetAllAsync() => db.Productos.AsNoTracking().OrderBy(x => x.Nombre).ToListAsync();
    public Task<Producto?> GetAsync(int id) => db.Productos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    public async Task<Producto> CreateAsync(ProductoRequest r) { var x = new Producto { Nombre = r.Nombre, Descripcion = r.Descripcion, Precio = r.Precio, Stock = r.Stock }; db.Add(x); await db.SaveChangesAsync(); return x; }
    public async Task<bool> UpdateAsync(int id, ProductoRequest r) { var x = await db.Productos.FindAsync(id); if (x is null) return false; x.Nombre = r.Nombre; x.Descripcion = r.Descripcion; x.Precio = r.Precio; x.Stock = r.Stock; await db.SaveChangesAsync(); return true; }
    public async Task<bool> DeleteAsync(int id) { var x = await db.Productos.FindAsync(id); if (x is null) return false; db.Remove(x); await db.SaveChangesAsync(); return true; }
}

public class PedidoService(PedidosDbContext db) : IPedidoService
{
    public async Task<List<PedidoResponse>> GetAllAsync() => await Project().ToListAsync();
    public Task<PedidoResponse?> GetAsync(int id) => Project().FirstOrDefaultAsync(x => x.Id == id);
    public async Task<PedidoResponse> CreateAsync(PedidoRequest r)
    {
        var cliente = await db.Clientes.FindAsync(r.ClienteId) ?? throw new BusinessRuleException("El cliente no existe.");
        var producto = await db.Productos.FindAsync(r.ProductoId) ?? throw new BusinessRuleException("El producto no existe.");
        if (producto.Stock < r.Cantidad) throw new BusinessRuleException("Stock insuficiente para crear el pedido.");
        producto.Stock -= r.Cantidad;
        var pedido = new Pedido { ClienteId = cliente.Id, ProductoId = producto.Id, Cantidad = r.Cantidad, FechaPedido = DateTime.UtcNow };
        db.Pedidos.Add(pedido); await db.SaveChangesAsync();
        return new PedidoResponse(pedido.Id, pedido.FechaPedido, pedido.Cantidad, cliente.Id, $"{cliente.Nombre} {cliente.Apellido}", producto.Id, producto.Nombre, producto.Precio * pedido.Cantidad);
    }
    private IQueryable<PedidoResponse> Project() => db.Pedidos.AsNoTracking().Include(x => x.Cliente).Include(x => x.Producto).OrderByDescending(x => x.FechaPedido).Select(x => new PedidoResponse(x.Id, x.FechaPedido, x.Cantidad, x.ClienteId, x.Cliente!.Nombre + " " + x.Cliente.Apellido, x.ProductoId, x.Producto!.Nombre, x.Producto.Precio * x.Cantidad));
}
