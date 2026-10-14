using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Pedidos.Application;
using Pedidos.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<PedidosDbContext>(o => o.UseSqlite(builder.Configuration.GetConnectionString("Pedidos") ?? "Data Source=pedidos.db"));
builder.Services.AddScoped<IClienteService, ClienteService>(); builder.Services.AddScoped<IProductoService, ProductoService>(); builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddControllers(); builder.Services.AddEndpointsApiExplorer(); builder.Services.AddSwaggerGen();
var app = builder.Build();
app.UseExceptionHandler(e => e.Run(async c => { var error = c.Features.Get<IExceptionHandlerFeature>()?.Error; c.Response.StatusCode = error is BusinessRuleException ? 400 : 500; await c.Response.WriteAsJsonAsync(new { error = error?.Message }); }));
app.UseSwagger(); app.UseSwaggerUI(); app.MapControllers();
using (var scope = app.Services.CreateScope()) { var db = scope.ServiceProvider.GetRequiredService<PedidosDbContext>(); db.Database.Migrate(); }
app.Run();
public partial class Program { }
