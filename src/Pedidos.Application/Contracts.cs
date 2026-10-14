using System.ComponentModel.DataAnnotations;

namespace Pedidos.Application;

public class ClienteRequest { [Required, StringLength(80, MinimumLength = 2)] public string Nombre { get; set; } = string.Empty; [Required, StringLength(80, MinimumLength = 2)] public string Apellido { get; set; } = string.Empty; [Required, EmailAddress, StringLength(120)] public string Email { get; set; } = string.Empty; }
public class ProductoRequest { [Required, StringLength(120, MinimumLength = 2)] public string Nombre { get; set; } = string.Empty; [Required, StringLength(500)] public string Descripcion { get; set; } = string.Empty; [Range(0.01, 999999.99)] public decimal Precio { get; set; } [Range(0, int.MaxValue)] public int Stock { get; set; } }
public class PedidoRequest { [Range(1, int.MaxValue)] public int ClienteId { get; set; } [Range(1, int.MaxValue)] public int ProductoId { get; set; } [Range(1, int.MaxValue)] public int Cantidad { get; set; } }
public record PedidoResponse(int Id, DateTime FechaPedido, int Cantidad, int ClienteId, string Cliente, int ProductoId, string Producto, decimal Total);
