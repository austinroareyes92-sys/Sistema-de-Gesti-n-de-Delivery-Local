using System.ComponentModel.DataAnnotations;

namespace DeliveryManagementAPI.AccesoDatos.Entidades;

public class RepartidorEntidad
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono es obligatorio")]
    [Phone]
    [StringLength(20)]
    public string Telefono { get; set; } = string.Empty;

    [Required(ErrorMessage = "El vehículo es obligatorio")]
    [StringLength(50)]
    public string Vehiculo { get; set; } = string.Empty;

    [StringLength(15)]
    public string? Placa { get; set; }

    public bool Disponible { get; set; } = true;

    // Relación 1 a N: un repartidor puede tener muchos pedidos asignados
    public ICollection<PedidoEntidad> Pedidos { get; set; } = new List<PedidoEntidad>();
}
