using DeliveryManagementAPI.Models;

namespace DeliveryManagementAPI.Service.Dtos;

public class PedidoServiceDTO
{
    public int? Id { get; set; }
    public int ClienteId { get; set; }
    public int? RepartidorId { get; set; }
    public string DireccionEntrega { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public string? Notas { get; set; }
    public EstadoPedido? Estado { get; set; }
}
