using System.ComponentModel.DataAnnotations;

namespace DeliveryManagementAPI.AccesoDatos.Modelos;

public class ClienteUpdateDTO
{
    [StringLength(100)]
    public string? Nombre { get; set; }

    [Phone]
    [StringLength(20)]
    public string? Telefono { get; set; }

    [StringLength(200)]
    public string? Direccion { get; set; }

    [EmailAddress]
    [StringLength(150)]
    public string? Email { get; set; }
}
