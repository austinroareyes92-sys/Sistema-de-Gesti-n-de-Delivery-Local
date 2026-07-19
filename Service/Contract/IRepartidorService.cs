using DeliveryManagementAPI.AccesoDatos.Entidades;

namespace DeliveryManagementAPI.Service.Contract;

public interface IRepartidorService
{
    Task<IEnumerable<RepartidorEntidad>> ObtenerTodosAsync();
    Task<RepartidorEntidad?> ObtenerPorIdAsync(int id);
    Task<IEnumerable<RepartidorEntidad>> ObtenerDisponiblesAsync();
    Task<RepartidorEntidad> CrearAsync(RepartidorEntidad repartidor);
    Task<RepartidorEntidad> ActualizarAsync(int id, RepartidorEntidad repartidor);
    Task<bool> EliminarAsync(int id);
    Task<bool> CambiarDisponibilidadAsync(int id, bool disponible);
}
