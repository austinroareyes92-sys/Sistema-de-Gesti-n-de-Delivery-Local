namespace DeliveryManagementAPI.AccesoDatos.Modelos;

public class RepartidorDTO
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Vehiculo { get; set; } = string.Empty;
    public string? Placa { get; set; }
    public bool Disponible { get; set; }
}
