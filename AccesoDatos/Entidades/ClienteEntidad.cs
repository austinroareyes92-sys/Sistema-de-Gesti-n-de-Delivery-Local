using System.ComponentModel.DataAnnotations;

namespace DeliveryManagementAPI.AccesoDatos.Entidades;

public class ClienteEntidad
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono es obligatorio")]
    [Phone]
    [StringLength(20)]
    public string Telefono { get; set; } = string.Empty;

    [Required(ErrorMessage = "La dirección es obligatoria")]
    [StringLength(200)]
    public string Direccion { get; set; } = string.Empty;

    [EmailAddress]
    [StringLength(150)]
    public string? Email { get; set; }

    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    // Relación 1 a N: un cliente puede tener muchos pedidos
    public ICollection<PedidoEntidad> Pedidos { get; set; } = new List<PedidoEntidad>();
}
