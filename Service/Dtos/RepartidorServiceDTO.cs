namespace DeliveryManagementAPI.Service.Dtos;

public class RepartidorServiceDTO
{
    public int? Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Vehiculo { get; set; } = string.Empty;
    public string? Placa { get; set; }
    public bool? Disponible { get; set; }
}
