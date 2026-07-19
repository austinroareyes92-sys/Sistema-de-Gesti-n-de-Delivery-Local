using System.ComponentModel.DataAnnotations;

namespace DeliveryManagementAPI.AccesoDatos.Modelos;

public class RepartidorCreateDTO
{
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
}
