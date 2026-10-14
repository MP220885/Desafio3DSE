using System.ComponentModel.DataAnnotations;

namespace Pedidos.Domain;

public class Cliente
{
    public int Id { get; set; }
    [Required, StringLength(80, MinimumLength = 2)] public string Nombre { get; set; } = string.Empty;
    [Required, StringLength(80, MinimumLength = 2)] public string Apellido { get; set; } = string.Empty;
    [Required, EmailAddress, StringLength(120)] public string Email { get; set; } = string.Empty;
    public List<Pedido> Pedidos { get; set; } = [];
}

public class Producto
{
    public int Id { get; set; }
    [Required, StringLength(120, MinimumLength = 2)] public string Nombre { get; set; } = string.Empty;
    [Required, StringLength(500)] public string Descripcion { get; set; } = string.Empty;
    [Range(0.01, 999999.99)] public decimal Precio { get; set; }
    [Range(0, int.MaxValue)] public int Stock { get; set; }
    public List<Pedido> Pedidos { get; set; } = [];
}

public class Pedido
{
    public int Id { get; set; }
    [Required] public DateTime FechaPedido { get; set; } = DateTime.UtcNow;
    [Range(1, int.MaxValue)] public int Cantidad { get; set; }
    [Range(1, int.MaxValue)] public int ClienteId { get; set; }
    [Range(1, int.MaxValue)] public int ProductoId { get; set; }
    public Cliente? Cliente { get; set; }
    public Producto? Producto { get; set; }
}
