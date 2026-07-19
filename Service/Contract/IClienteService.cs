using DeliveryManagementAPI.AccesoDatos.Entidades;

namespace DeliveryManagementAPI.Service.Contract;

public interface IClienteService
{
    Task<IEnumerable<ClienteEntidad>> ObtenerTodosAsync();
    Task<ClienteEntidad?> ObtenerPorIdAsync(int id);
    Task<IEnumerable<ClienteEntidad>> BuscarPorNombreAsync(string nombre);
    Task<ClienteEntidad> CrearAsync(ClienteEntidad cliente);
    Task<ClienteEntidad> ActualizarAsync(int id, ClienteEntidad cliente);
    Task<bool> EliminarAsync(int id);
}
