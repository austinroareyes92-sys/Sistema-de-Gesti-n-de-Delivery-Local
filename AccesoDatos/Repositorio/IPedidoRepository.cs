using DeliveryManagementAPI.AccesoDatos.Entidades;
using DeliveryManagementAPI.Models;

namespace DeliveryManagementAPI.AccesoDatos.Repositorio;

public interface IPedidoRepository
{
    Task<IEnumerable<PedidoEntidad>> ObtenerTodosAsync();
    Task<IEnumerable<PedidoEntidad>> ObtenerPorEstadoAsync(EstadoPedido estado);
    Task<PedidoEntidad?> ObtenerPorIdAsync(int id);
    Task<IEnumerable<PedidoEntidad>> ObtenerPorClienteAsync(int clienteId);
    Task<IEnumerable<PedidoEntidad>> ObtenerPorRepartidorAsync(int repartidorId);
    Task<PedidoEntidad> CrearAsync(PedidoEntidad pedido);
    Task<PedidoEntidad> ActualizarAsync(PedidoEntidad pedido);
    Task<bool> EliminarAsync(int id);
    Task<bool> GuardarCambiosAsync();
}
