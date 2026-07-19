using DeliveryManagementAPI.AccesoDatos.Entidades;
using DeliveryManagementAPI.Models;

namespace DeliveryManagementAPI.Service.Contract;

public interface IPedidoService
{
    Task<IEnumerable<PedidoEntidad>> ObtenerTodosAsync();
    Task<IEnumerable<PedidoEntidad>> ObtenerPorEstadoAsync(EstadoPedido estado);
    Task<PedidoEntidad?> ObtenerPorIdAsync(int id);
    Task<IEnumerable<PedidoEntidad>> ObtenerPorClienteAsync(int clienteId);
    Task<IEnumerable<PedidoEntidad>> ObtenerPorRepartidorAsync(int repartidorId);
    Task<PedidoEntidad> CrearAsync(PedidoEntidad pedido);
    Task<PedidoEntidad> ActualizarAsync(int id, PedidoEntidad pedido);
    Task<bool> EliminarAsync(int id);
    Task<PedidoEntidad> AsignarRepartidorAsync(int pedidoId, int repartidorId);
    Task<PedidoEntidad> CambiarEstadoAsync(int pedidoId, EstadoPedido nuevoEstado);
}
