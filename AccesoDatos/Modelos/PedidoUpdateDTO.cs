using System.ComponentModel.DataAnnotations;
using DeliveryManagementAPI.Models;

namespace DeliveryManagementAPI.AccesoDatos.Modelos;

public class PedidoUpdateDTO
{
    public int? RepartidorId { get; set; }

    [StringLength(200)]
    public string? DireccionEntrega { get; set; }

    public EstadoPedido? Estado { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "El total debe ser mayor a 0")]
    public decimal? Total { get; set; }

    [StringLength(500)]
    public string? Notas { get; set; }
}
