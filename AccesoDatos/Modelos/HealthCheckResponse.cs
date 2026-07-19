namespace DeliveryManagementAPI.AccesoDatos.Modelos;

public class HealthCheckResponse
{
    public string Status { get; set; } = string.Empty;
    public string Database { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
