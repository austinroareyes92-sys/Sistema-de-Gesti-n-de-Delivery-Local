using System.ComponentModel.DataAnnotations;
using DeliveryManagementAPI.Models;

namespace DeliveryManagementAPI.DTOs;


public class PedidoDTO
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public string ClienteNombre { get; set; } = string.Empty;
    public int? RepartidorId { get; set; }
    public string? RepartidorNombre { get; set; }
    public string DireccionEntrega { get; set; } = string.Empty;
    public DateTime FechaPedido { get; set; }
    public DateTime? FechaEntrega { get; set; }
    public string Estado { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public string? Notas { get; set; }
}


public class PedidoCreateDTO
{
    [Required(ErrorMessage = "El cliente es obligatorio")]
    public int ClienteId { get; set; }

    [Required(ErrorMessage = "La dirección de entrega es obligatoria")]
    [StringLength(200)]
    public string DireccionEntrega { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "El total debe ser mayor a 0")]
    public decimal Total { get; set; }

    [StringLength(500)]
    public string? Notas { get; set; }
}


public class PedidoUpdateDTO
{
    [Required(ErrorMessage = "La dirección de entrega es obligatoria")]
    [StringLength(200)]
    public string DireccionEntrega { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "El total debe ser mayor a 0")]
    public decimal Total { get; set; }

    [StringLength(500)]
    public string? Notas { get; set; }
}


public class PedidoEstadoDTO
{
    [Required(ErrorMessage = "El estado es obligatorio")]
    public EstadoPedido Estado { get; set; }

    public int? RepartidorId { get; set; }
}
