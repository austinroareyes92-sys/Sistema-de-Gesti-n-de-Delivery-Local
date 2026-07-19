using System.ComponentModel.DataAnnotations;

namespace DeliveryManagementAPI.AccesoDatos.Modelos;

public class RepartidorUpdateDTO
{
    [StringLength(100)]
    public string? Nombre { get; set; }

    [Phone]
    [StringLength(20)]
    public string? Telefono { get; set; }

    [StringLength(50)]
    public string? Vehiculo { get; set; }

    [StringLength(15)]
    public string? Placa { get; set; }

    public bool? Disponible { get; set; }
}
