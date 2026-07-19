using DeliveryManagementAPI.Models;

namespace DeliveryManagementAPI.AccesoDatos.Modelos;

public class PedidoDTO
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public int? RepartidorId { get; set; }
    public string DireccionEntrega { get; set; } = string.Empty;
    public DateTime FechaPedido { get; set; }
    public DateTime? FechaEntrega { get; set; }
    public EstadoPedido Estado { get; set; }
    public decimal Total { get; set; }
    public string? Notas { get; set; }
}
