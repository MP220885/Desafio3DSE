using Pedidos.Domain;

namespace Pedidos.Application;

public interface IClienteService { Task<List<Cliente>> GetAllAsync(); Task<Cliente?> GetAsync(int id); Task<Cliente> CreateAsync(ClienteRequest request); Task<bool> UpdateAsync(int id, ClienteRequest request); Task<bool> DeleteAsync(int id); }
public interface IProductoService { Task<List<Producto>> GetAllAsync(); Task<Producto?> GetAsync(int id); Task<Producto> CreateAsync(ProductoRequest request); Task<bool> UpdateAsync(int id, ProductoRequest request); Task<bool> DeleteAsync(int id); }
public interface IPedidoService { Task<List<PedidoResponse>> GetAllAsync(); Task<PedidoResponse?> GetAsync(int id); Task<PedidoResponse> CreateAsync(PedidoRequest request); }
public sealed class BusinessRuleException(string message) : Exception(message);
