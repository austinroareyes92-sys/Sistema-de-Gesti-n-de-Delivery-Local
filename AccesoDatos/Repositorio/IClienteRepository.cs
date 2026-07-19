using DeliveryManagementAPI.AccesoDatos.Entidades;
using DeliveryManagementAPI.AccesoDatos.Modelos;

namespace DeliveryManagementAPI.AccesoDatos.Repositorio;

public interface IClienteRepository
{
    Task<IEnumerable<ClienteEntidad>> ObtenerTodosAsync();
    Task<ClienteEntidad?> ObtenerPorIdAsync(int id);
    Task<IEnumerable<ClienteEntidad>> BuscarPorNombreAsync(string nombre);
    Task<ClienteEntidad> CrearAsync(ClienteEntidad cliente);
    Task<ClienteEntidad> ActualizarAsync(ClienteEntidad cliente);
    Task<bool> EliminarAsync(int id);
    Task<bool> GuardarCambiosAsync();
}
