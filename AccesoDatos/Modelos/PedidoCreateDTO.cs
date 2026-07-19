using System.ComponentModel.DataAnnotations;
using DeliveryManagementAPI.Models;

namespace DeliveryManagementAPI.AccesoDatos.Modelos;

public class PedidoCreateDTO
{
    [Required]
    public int ClienteId { get; set; }

    public int? RepartidorId { get; set; }

    [Required(ErrorMessage = "La dirección de entrega es obligatoria")]
    [StringLength(200)]
    public string DireccionEntrega { get; set; } = string.Empty;

    [Required(ErrorMessage = "El total es obligatorio")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El total debe ser mayor a 0")]
    public decimal Total { get; set; }

    [StringLength(500)]
    public string? Notas { get; set; }
}
