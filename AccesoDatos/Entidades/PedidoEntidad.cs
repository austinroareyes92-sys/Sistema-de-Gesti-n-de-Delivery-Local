using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DeliveryManagementAPI.Models;

namespace DeliveryManagementAPI.AccesoDatos.Entidades;

public class PedidoEntidad
{
    public int Id { get; set; }

    [Required]
    public int ClienteId { get; set; }
    public ClienteEntidad? Cliente { get; set; }

    // Nullable: el pedido puede no tener repartidor asignado todavía
    public int? RepartidorId { get; set; }
    public RepartidorEntidad? Repartidor { get; set; }

    [Required(ErrorMessage = "La dirección de entrega es obligatoria")]
    [StringLength(200)]
    public string DireccionEntrega { get; set; } = string.Empty;

    public DateTime FechaPedido { get; set; } = DateTime.UtcNow;

    public DateTime? FechaEntrega { get; set; }

    public EstadoPedido Estado { get; set; } = EstadoPedido.Pendiente;

    [Column(TypeName = "decimal(10,2)")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El total debe ser mayor a 0")]
    public decimal Total { get; set; }

    [StringLength(500)]
    public string? Notas { get; set; }
}
