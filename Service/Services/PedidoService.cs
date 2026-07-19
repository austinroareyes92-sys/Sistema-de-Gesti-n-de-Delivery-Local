using DeliveryManagementAPI.AccesoDatos.Entidades;
using DeliveryManagementAPI.AccesoDatos.Repositorio;
using DeliveryManagementAPI.Models;
using DeliveryManagementAPI.Service.Contract;
using DeliveryManagementAPI.Service.Exceptions;

namespace DeliveryManagementAPI.Service.Services;

public class PedidoService : IPedidoService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IRepartidorRepository _repartidorRepository;

    public PedidoService(
        IPedidoRepository pedidoRepository,
        IClienteRepository clienteRepository,
        IRepartidorRepository repartidorRepository)
    {
        _pedidoRepository = pedidoRepository;
        _clienteRepository = clienteRepository;
        _repartidorRepository = repartidorRepository;
    }

    public async Task<IEnumerable<PedidoEntidad>> ObtenerTodosAsync()
    {
        try
        {
            return await _pedidoRepository.ObtenerTodosAsync();
        }
        catch (Exception ex)
        {
            throw new PedidoServiceException("Error al obtener todos los pedidos", ex);
        }
    }

    public async Task<IEnumerable<PedidoEntidad>> ObtenerPorEstadoAsync(EstadoPedido estado)
    {
        try
        {
            return await _pedidoRepository.ObtenerPorEstadoAsync(estado);
        }
        catch (Exception ex)
        {
            throw new PedidoServiceException($"Error al obtener pedidos con estado {estado}", ex);
        }
    }

    public async Task<PedidoEntidad?> ObtenerPorIdAsync(int id)
    {
        try
        {
            if (id <= 0)
                throw new ArgumentException("El ID del pedido debe ser mayor a 0");

            return await _pedidoRepository.ObtenerPorIdAsync(id);
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new PedidoServiceException($"Error al obtener el pedido con ID {id}", ex);
        }
    }

    public async Task<IEnumerable<PedidoEntidad>> ObtenerPorClienteAsync(int clienteId)
    {
        try
        {
            if (clienteId <= 0)
                throw new ArgumentException("El ID del cliente debe ser mayor a 0");

            var cliente = await _clienteRepository.ObtenerPorIdAsync(clienteId);
            if (cliente == null)
                throw new PedidoServiceException($"El cliente con ID {clienteId} no existe");

            return await _pedidoRepository.ObtenerPorClienteAsync(clienteId);
        }
        catch (PedidoServiceException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new PedidoServiceException($"Error al obtener pedidos del cliente {clienteId}", ex);
        }
    }

    public async Task<IEnumerable<PedidoEntidad>> ObtenerPorRepartidorAsync(int repartidorId)
    {
        try
        {
            if (repartidorId <= 0)
                throw new ArgumentException("El ID del repartidor debe ser mayor a 0");

            var repartidor = await _repartidorRepository.ObtenerPorIdAsync(repartidorId);
            if (repartidor == null)
                throw new PedidoServiceException($"El repartidor con ID {repartidorId} no existe");

            return await _pedidoRepository.ObtenerPorRepartidorAsync(repartidorId);
        }
        catch (PedidoServiceException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new PedidoServiceException($"Error al obtener pedidos del repartidor {repartidorId}", ex);
        }
    }

    public async Task<PedidoEntidad> CrearAsync(PedidoEntidad pedido)
    {
        try
        {
            ValidarPedido(pedido);

            // Verificar que el cliente existe
            var cliente = await _clienteRepository.ObtenerPorIdAsync(pedido.ClienteId);
            if (cliente == null)
                throw new PedidoServiceException($"El cliente con ID {pedido.ClienteId} no existe");

            // Verificar repartidor si está especificado
            if (pedido.RepartidorId.HasValue)
            {
                var repartidor = await _repartidorRepository.ObtenerPorIdAsync(pedido.RepartidorId.Value);
                if (repartidor == null)
                    throw new PedidoServiceException($"El repartidor con ID {pedido.RepartidorId.Value} no existe");

                if (!repartidor.Disponible)
                    throw new PedidoServiceException($"El repartidor con ID {pedido.RepartidorId.Value} no está disponible");
            }

            pedido.FechaPedido = DateTime.UtcNow;
            pedido.Estado = EstadoPedido.Pendiente;

            return await _pedidoRepository.CrearAsync(pedido);
        }
        catch (PedidoServiceException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new PedidoServiceException("Error al crear el pedido", ex);
        }
    }

    public async Task<PedidoEntidad> ActualizarAsync(int id, PedidoEntidad pedido)
    {
        try
        {
            if (id <= 0)
                throw new ArgumentException("El ID del pedido debe ser mayor a 0");

            var pedidoExistente = await _pedidoRepository.ObtenerPorIdAsync(id);
            if (pedidoExistente == null)
                throw new PedidoServiceException($"El pedido con ID {id} no existe");

            ValidarPedido(pedido);

            // Verificar que el cliente existe
            var cliente = await _clienteRepository.ObtenerPorIdAsync(pedido.ClienteId);
            if (cliente == null)
                throw new PedidoServiceException($"El cliente con ID {pedido.ClienteId} no existe");

            // Verificar repartidor si está especificado
            if (pedido.RepartidorId.HasValue)
            {
                var repartidor = await _repartidorRepository.ObtenerPorIdAsync(pedido.RepartidorId.Value);
                if (repartidor == null)
                    throw new PedidoServiceException($"El repartidor con ID {pedido.RepartidorId.Value} no existe");
            }

            pedidoExistente.DireccionEntrega = pedido.DireccionEntrega;
            pedidoExistente.Total = pedido.Total;
            pedidoExistente.Notas = pedido.Notas;
            pedidoExistente.RepartidorId = pedido.RepartidorId;
            pedidoExistente.Estado = pedido.Estado;

            return await _pedidoRepository.ActualizarAsync(pedidoExistente);
        }
        catch (PedidoServiceException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new PedidoServiceException($"Error al actualizar el pedido con ID {id}", ex);
        }
    }

    public async Task<bool> EliminarAsync(int id)
    {
        try
        {
            if (id <= 0)
                throw new ArgumentException("El ID del pedido debe ser mayor a 0");

            var pedido = await _pedidoRepository.ObtenerPorIdAsync(id);
            if (pedido == null)
                throw new PedidoServiceException($"El pedido con ID {id} no existe");

            // No permitir eliminar pedidos en ciertos estados
            if (pedido.Estado == EstadoPedido.Entregado || pedido.Estado == EstadoPedido.EnTransito)
                throw new PedidoServiceException($"No se puede eliminar un pedido en estado {pedido.Estado}");

            return await _pedidoRepository.EliminarAsync(id);
        }
        catch (PedidoServiceException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new PedidoServiceException($"Error al eliminar el pedido con ID {id}", ex);
        }
    }

    public async Task<PedidoEntidad> AsignarRepartidorAsync(int pedidoId, int repartidorId)
    {
        try
        {
            if (pedidoId <= 0)
                throw new ArgumentException("El ID del pedido debe ser mayor a 0");
            if (repartidorId <= 0)
                throw new ArgumentException("El ID del repartidor debe ser mayor a 0");

            var pedido = await _pedidoRepository.ObtenerPorIdAsync(pedidoId);
            if (pedido == null)
                throw new PedidoServiceException($"El pedido con ID {pedidoId} no existe");

            var repartidor = await _repartidorRepository.ObtenerPorIdAsync(repartidorId);
            if (repartidor == null)
                throw new PedidoServiceException($"El repartidor con ID {repartidorId} no existe");

            if (!repartidor.Disponible)
                throw new PedidoServiceException($"El repartidor con ID {repartidorId} no está disponible");

            if (pedido.Estado != EstadoPedido.Pendiente)
                throw new PedidoServiceException($"Solo se pueden asignar repartidores a pedidos en estado Pendiente");

            pedido.RepartidorId = repartidorId;
            pedido.Estado = EstadoPedido.Confirmado;

            return await _pedidoRepository.ActualizarAsync(pedido);
        }
        catch (PedidoServiceException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new PedidoServiceException($"Error al asignar repartidor al pedido {pedidoId}", ex);
        }
    }

    public async Task<PedidoEntidad> CambiarEstadoAsync(int pedidoId, EstadoPedido nuevoEstado)
    {
        try
        {
            if (pedidoId <= 0)
                throw new ArgumentException("El ID del pedido debe ser mayor a 0");

            var pedido = await _pedidoRepository.ObtenerPorIdAsync(pedidoId);
            if (pedido == null)
                throw new PedidoServiceException($"El pedido con ID {pedidoId} no existe");

            // Validar transiciones de estado
            if (!EsTransicionEstadoValida(pedido.Estado, nuevoEstado))
                throw new PedidoServiceException($"No se puede cambiar de estado {pedido.Estado} a {nuevoEstado}");

            // Si pasa a Entregado, registrar fecha de entrega
            if (nuevoEstado == EstadoPedido.Entregado)
                pedido.FechaEntrega = DateTime.UtcNow;

            pedido.Estado = nuevoEstado;
            return await _pedidoRepository.ActualizarAsync(pedido);
        }
        catch (PedidoServiceException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new PedidoServiceException($"Error al cambiar estado del pedido {pedidoId}", ex);
        }
    }

    private void ValidarPedido(PedidoEntidad pedido)
    {
        if (pedido == null)
            throw new ArgumentNullException(nameof(pedido));

        if (pedido.ClienteId <= 0)
            throw new ArgumentException("El ID del cliente debe ser mayor a 0");

        if (string.IsNullOrWhiteSpace(pedido.DireccionEntrega))
            throw new ArgumentException("La dirección de entrega es obligatoria");

        if (pedido.DireccionEntrega.Length < 5 || pedido.DireccionEntrega.Length > 200)
            throw new ArgumentException("La dirección debe tener entre 5 y 200 caracteres");

        if (pedido.Total <= 0)
            throw new ArgumentException("El total del pedido debe ser mayor a 0");

        if (pedido.Total > decimal.MaxValue / 2)
            throw new ArgumentException("El total del pedido es demasiado grande");
    }

    private bool EsTransicionEstadoValida(EstadoPedido estadoActual, EstadoPedido nuevoEstado)
    {
        // Validar transiciones permitidas
        var transicionesValidas = new Dictionary<EstadoPedido, List<EstadoPedido>>
        {
            { EstadoPedido.Pendiente, new List<EstadoPedido> { EstadoPedido.Confirmado, EstadoPedido.Cancelado } },
            { EstadoPedido.Confirmado, new List<EstadoPedido> { EstadoPedido.EnTransito, EstadoPedido.Cancelado } },
            { EstadoPedido.EnTransito, new List<EstadoPedido> { EstadoPedido.Entregado, EstadoPedido.Cancelado } },
            { EstadoPedido.Entregado, new List<EstadoPedido> { } },
            { EstadoPedido.Cancelado, new List<EstadoPedido> { } }
        };

        return transicionesValidas.ContainsKey(estadoActual) && transicionesValidas[estadoActual].Contains(nuevoEstado);
    }
}
