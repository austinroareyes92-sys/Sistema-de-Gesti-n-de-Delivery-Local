using Microsoft.AspNetCore.Mvc;
using DeliveryManagementAPI.AccesoDatos.Entidades;
using DeliveryManagementAPI.AccesoDatos.Modelos;
using DeliveryManagementAPI.AccesoDatos.Repositorio;
using DeliveryManagementAPI.Models;

namespace DeliveryManagementAPI.Controladores;

[ApiController]
[Route("api/[controller]")]
public class PedidosController : ControllerBase
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IRepartidorRepository _repartidorRepository;

    public PedidosController(
        IPedidoRepository pedidoRepository,
        IClienteRepository clienteRepository,
        IRepartidorRepository repartidorRepository)
    {
        _pedidoRepository = pedidoRepository;
        _clienteRepository = clienteRepository;
        _repartidorRepository = repartidorRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PedidoDTO>>> ObtenerTodos([FromQuery] string? estado = null)
    {
        IEnumerable<PedidoEntidad> pedidos;

        if (!string.IsNullOrWhiteSpace(estado))
        {
            // Validar que el estado sea válido
            if (!Enum.TryParse<EstadoPedido>(estado, true, out var estadoEnum))
                return BadRequest(new { mensaje = $"Estado '{estado}' no válido. Estados válidos: {string.Join(", ", Enum.GetNames(typeof(EstadoPedido)))}" });

            pedidos = await _pedidoRepository.ObtenerPorEstadoAsync(estadoEnum);
        }
        else
        {
            pedidos = await _pedidoRepository.ObtenerTodosAsync();
        }

        return Ok(pedidos.Select(MapToDTO));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PedidoDTO>> ObtenerPorId(int id)
    {
        var pedido = await _pedidoRepository.ObtenerPorIdAsync(id);
        if (pedido == null)
            return NotFound(new { mensaje = "Pedido no encontrado" });

        return Ok(MapToDTO(pedido));
    }

    [HttpPost]
    public async Task<ActionResult<PedidoDTO>> Crear([FromBody] PedidoCreateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Validar que el cliente exista
        var cliente = await _clienteRepository.ObtenerPorIdAsync(dto.ClienteId);
        if (cliente == null)
            return BadRequest(new { mensaje = "El cliente especificado no existe" });

        // Validar que el repartidor exista si está especificado
        if (dto.RepartidorId.HasValue)
        {
            var repartidor = await _repartidorRepository.ObtenerPorIdAsync(dto.RepartidorId.Value);
            if (repartidor == null)
                return BadRequest(new { mensaje = "El repartidor especificado no existe" });
        }

        var pedido = new PedidoEntidad
        {
            ClienteId = dto.ClienteId,
            RepartidorId = dto.RepartidorId,
            DireccionEntrega = dto.DireccionEntrega,
            Total = dto.Total,
            Notas = dto.Notas,
            Estado = EstadoPedido.Pendiente
        };

        var pedidoCreado = await _pedidoRepository.CrearAsync(pedido);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = pedidoCreado.Id }, MapToDTO(pedidoCreado));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PedidoDTO>> Actualizar(int id, [FromBody] PedidoUpdateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var pedido = await _pedidoRepository.ObtenerPorIdAsync(id);
        if (pedido == null)
            return NotFound(new { mensaje = "Pedido no encontrado" });

        // Validar repartidor si se está actualizando
        if (dto.RepartidorId.HasValue)
        {
            var repartidor = await _repartidorRepository.ObtenerPorIdAsync(dto.RepartidorId.Value);
            if (repartidor == null)
                return BadRequest(new { mensaje = "El repartidor especificado no existe" });
            pedido.RepartidorId = dto.RepartidorId.Value;
        }

        if (!string.IsNullOrWhiteSpace(dto.DireccionEntrega))
            pedido.DireccionEntrega = dto.DireccionEntrega;
        if (dto.Estado.HasValue)
            pedido.Estado = dto.Estado.Value;
        if (dto.Total.HasValue)
            pedido.Total = dto.Total.Value;
        if (dto.Notas != null)
            pedido.Notas = dto.Notas;

        var pedidoActualizado = await _pedidoRepository.ActualizarAsync(pedido);
        return Ok(MapToDTO(pedidoActualizado));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Eliminar(int id)
    {
        var resultado = await _pedidoRepository.EliminarAsync(id);
        if (!resultado)
            return NotFound(new { mensaje = "Pedido no encontrado" });

        return NoContent();
    }

    [HttpGet("cliente/{clienteId}")]
    public async Task<ActionResult<IEnumerable<PedidoDTO>>> ObtenerPorCliente(int clienteId)
    {
        var pedidos = await _pedidoRepository.ObtenerPorClienteAsync(clienteId);
        return Ok(pedidos.Select(MapToDTO));
    }

    [HttpGet("repartidor/{repartidorId}")]
    public async Task<ActionResult<IEnumerable<PedidoDTO>>> ObtenerPorRepartidor(int repartidorId)
    {
        var pedidos = await _pedidoRepository.ObtenerPorRepartidorAsync(repartidorId);
        return Ok(pedidos.Select(MapToDTO));
    }

    private PedidoDTO MapToDTO(PedidoEntidad pedido)
    {
        return new PedidoDTO
        {
            Id = pedido.Id,
            ClienteId = pedido.ClienteId,
            RepartidorId = pedido.RepartidorId,
            DireccionEntrega = pedido.DireccionEntrega,
            FechaPedido = pedido.FechaPedido,
            FechaEntrega = pedido.FechaEntrega,
            Estado = pedido.Estado,
            Total = pedido.Total,
            Notas = pedido.Notas
        };
    }
}
