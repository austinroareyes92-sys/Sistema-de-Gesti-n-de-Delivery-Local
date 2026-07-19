using DeliveryManagementAPI.AccesoDatos.Entidades;

namespace DeliveryManagementAPI.AccesoDatos.Repositorio;

public interface IRepartidorRepository
{
    Task<IEnumerable<RepartidorEntidad>> ObtenerTodosAsync();
    Task<RepartidorEntidad?> ObtenerPorIdAsync(int id);
    Task<IEnumerable<RepartidorEntidad>> ObtenerDisponiblesAsync();
    Task<RepartidorEntidad> CrearAsync(RepartidorEntidad repartidor);
    Task<RepartidorEntidad> ActualizarAsync(RepartidorEntidad repartidor);
    Task<bool> EliminarAsync(int id);
    Task<bool> GuardarCambiosAsync();
}
