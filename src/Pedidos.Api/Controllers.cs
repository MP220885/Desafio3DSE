using Microsoft.AspNetCore.Mvc;
using Pedidos.Application;
using Pedidos.Domain;

namespace Pedidos.Api.Controllers;

[ApiController, Route("api/clientes")]
public class ClientesController(IClienteService service) : ControllerBase
{
    [HttpGet] public Task<List<Cliente>> Get() => service.GetAllAsync();
    [HttpGet("{id:int}")] public async Task<ActionResult<Cliente>> Get(int id) => await service.GetAsync(id) is { } x ? Ok(x) : NotFound();
    [HttpPost] public async Task<ActionResult<Cliente>> Post(ClienteRequest r) { var x = await service.CreateAsync(r); return CreatedAtAction(nameof(Get), new { id = x.Id }, x); }
    [HttpPut("{id:int}")] public async Task<IActionResult> Put(int id, ClienteRequest r) => await service.UpdateAsync(id, r) ? NoContent() : NotFound();
    [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id) => await service.DeleteAsync(id) ? NoContent() : NotFound();
}

[ApiController, Route("api/productos")]
public class ProductosController(IProductoService service) : ControllerBase
{
    [HttpGet] public Task<List<Producto>> Get() => service.GetAllAsync();
    [HttpGet("{id:int}")] public async Task<ActionResult<Producto>> Get(int id) => await service.GetAsync(id) is { } x ? Ok(x) : NotFound();
    [HttpPost] public async Task<ActionResult<Producto>> Post(ProductoRequest r) { var x = await service.CreateAsync(r); return CreatedAtAction(nameof(Get), new { id = x.Id }, x); }
    [HttpPut("{id:int}")] public async Task<IActionResult> Put(int id, ProductoRequest r) => await service.UpdateAsync(id, r) ? NoContent() : NotFound();
    [HttpDelete("{id:int}")] public async Task<IActionResult> Delete(int id) => await service.DeleteAsync(id) ? NoContent() : NotFound();
}

[ApiController, Route("api/pedidos")]
public class PedidosController(IPedidoService service) : ControllerBase
{
    [HttpGet] public Task<List<PedidoResponse>> Get() => service.GetAllAsync();
    [HttpGet("{id:int}")] public async Task<ActionResult<PedidoResponse>> Get(int id) => await service.GetAsync(id) is { } x ? Ok(x) : NotFound();
    [HttpPost] public async Task<ActionResult<PedidoResponse>> Post(PedidoRequest r) { var x = await service.CreateAsync(r); return CreatedAtAction(nameof(Get), new { id = x.Id }, x); }
}
