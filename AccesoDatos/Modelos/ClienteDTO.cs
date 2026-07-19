namespace DeliveryManagementAPI.AccesoDatos.Modelos;

public class ClienteDTO
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string? Email { get; set; }
    public DateTime FechaRegistro { get; set; }
}
