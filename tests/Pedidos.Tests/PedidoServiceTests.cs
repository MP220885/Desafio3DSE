using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Pedidos.Application;
using Pedidos.Domain;
using Pedidos.Infrastructure;
using Xunit;

namespace Pedidos.Tests;

public class PedidoServiceTests
{
    private static PedidosDbContext Db() => new(new DbContextOptionsBuilder<PedidosDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
    private static ClienteRequest Cliente() => new() { Nombre = "Ana", Apellido = "López", Email = "ana@correo.com" };
    private static ProductoRequest Producto(decimal precio = 10m, int stock = 5) => new() { Nombre = "Mouse", Descripcion = "Mouse óptico", Precio = precio, Stock = stock };
    [Fact] public async Task Crea_cliente() { await using var db = Db(); var c = await new ClienteService(db).CreateAsync(Cliente()); Assert.True(c.Id > 0); }
    [Fact] public void Rechaza_cliente_con_email_invalido() { var value = new ClienteRequest { Nombre = "A", Email = "no-es-email" }; var results = new List<ValidationResult>(); Assert.False(Validator.TryValidateObject(value, new(value), results, true)); }
    [Fact] public async Task Crea_producto() { await using var db = Db(); var p = await new ProductoService(db).CreateAsync(new ProductoRequest { Nombre = "Teclado", Descripcion = "Teclado mecánico", Precio = 45.50m, Stock = 10 }); Assert.Equal(10, p.Stock); }
    [Theory, InlineData(0, 2), InlineData(10, -1)] public void Rechaza_precio_o_stock_invalidos(decimal precio, int stock) { var value = Producto(precio, stock); Assert.False(Validator.TryValidateObject(value, new(value), [], true)); }
    [Fact] public async Task Crea_pedido_y_descuenta_stock() { await using var db = Db(); var c = await new ClienteService(db).CreateAsync(Cliente()); var p = await new ProductoService(db).CreateAsync(Producto()); var pedido = await new PedidoService(db).CreateAsync(new PedidoRequest { ClienteId = c.Id, ProductoId = p.Id, Cantidad = 2 }); Assert.Equal(2, pedido.Cantidad); Assert.Equal(3, (await db.Productos.FindAsync(p.Id))!.Stock); }
    [Fact] public async Task Consulta_pedido_existente() { await using var db = Db(); var c = await new ClienteService(db).CreateAsync(Cliente()); var p = await new ProductoService(db).CreateAsync(Producto()); var creado = await new PedidoService(db).CreateAsync(new PedidoRequest { ClienteId = c.Id, ProductoId = p.Id, Cantidad = 1 }); Assert.NotNull(await new PedidoService(db).GetAsync(creado.Id)); }
    [Fact] public async Task Pedido_inexistente_retorna_nulo() { await using var db = Db(); Assert.Null(await new PedidoService(db).GetAsync(99)); }
    [Fact] public async Task Impide_pedido_sin_stock_suficiente() { await using var db = Db(); var c = await new ClienteService(db).CreateAsync(Cliente()); var p = await new ProductoService(db).CreateAsync(Producto(stock: 1)); await Assert.ThrowsAsync<BusinessRuleException>(() => new PedidoService(db).CreateAsync(new PedidoRequest { ClienteId = c.Id, ProductoId = p.Id, Cantidad = 2 })); }
}
