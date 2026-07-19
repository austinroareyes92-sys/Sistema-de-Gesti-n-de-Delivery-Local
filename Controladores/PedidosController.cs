using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeliveryManagementAPI.Data;
using DeliveryManagementAPI.DTOs;
using DeliveryManagementAPI.Models;

namespace DeliveryManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidosController : ControllerBase
{
    private readonly DeliveryContext _context;

    public PedidosController(DeliveryContext context)
    {
        _context = context;
    }

    private static PedidoDTO ToDTO(Pedido p) => new()
    {
        Id = p.Id,
        ClienteId = p.ClienteId,
        ClienteNombre = p.Cliente?.Nombre ?? string.Empty,
        RepartidorId = p.RepartidorId,
        RepartidorNombre = p.Repartidor?.Nombre,
        DireccionEntrega = p.DireccionEntrega,
        FechaPedido = p.FechaPedido,
        FechaEntrega = p.FechaEntrega,
        Estado = p.Estado.ToString(),
        Total = p.Total,
        Notas = p.Notas
    };

    // GET: api/pedidos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PedidoDTO>>> GetPedidos()
    {
        var pedidos = await _context.Pedidos
            .Include(p => p.Cliente)
            .Include(p => p.Repartidor)
            .OrderByDescending(p => p.FechaPedido)
            .ToListAsync();

        return Ok(pedidos.Select(ToDTO));
    }

    // GET: api/pedidos/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PedidoDTO>> GetPedido(int id)
    {
        var pedido = await _context.Pedidos
            .Include(p => p.Cliente)
            .Include(p => p.Repartidor)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pedido == null)
            return NotFound($"No se encontró el pedido con Id {id}");

        return Ok(ToDTO(pedido));
    }

    // GET: api/pedidos/cliente/3
    [HttpGet("cliente/{clienteId:int}")]
    public async Task<ActionResult<IEnumerable<PedidoDTO>>> GetPedidosPorCliente(int clienteId)
    {
        var existeCliente = await _context.Clientes.AnyAsync(c => c.Id == clienteId);
        if (!existeCliente)
            return NotFound($"No se encontró el cliente con Id {clienteId}");

        var pedidos = await _context.Pedidos
            .Include(p => p.Cliente)
            .Include(p => p.Repartidor)
            .Where(p => p.ClienteId == clienteId)
            .OrderByDescending(p => p.FechaPedido)
            .ToListAsync();

        return Ok(pedidos.Select(ToDTO));
    }

    // POST: api/pedidos
    [HttpPost]
    public async Task<ActionResult<PedidoDTO>> CrearPedido(PedidoCreateDTO dto)
    {
        var clienteExiste = await _context.Clientes.AnyAsync(c => c.Id == dto.ClienteId);
        if (!clienteExiste)
            return BadRequest($"No existe un cliente con Id {dto.ClienteId}");

        var pedido = new Pedido
        {
            ClienteId = dto.ClienteId,
            DireccionEntrega = dto.DireccionEntrega,
            Total = dto.Total,
            Notas = dto.Notas,
            FechaPedido = DateTime.UtcNow,
            Estado = EstadoPedido.Pendiente
        };

        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();

        // Se carga la navegación de Cliente para poder devolver su nombre en el DTO
        await _context.Entry(pedido).Reference(p => p.Cliente).LoadAsync();

        return CreatedAtAction(nameof(GetPedido), new { id = pedido.Id }, ToDTO(pedido));
    }

    // PUT: api/pedidos/5  (datos generales, no cambia estado ni repartidor)
    [HttpPut("{id:int}")]
    public async Task<IActionResult> ActualizarPedido(int id, PedidoUpdateDTO dto)
    {
        var pedido = await _context.Pedidos.FindAsync(id);

        if (pedido == null)
            return NotFound($"No se encontró el pedido con Id {id}");

        if (pedido.Estado is EstadoPedido.Entregado or EstadoPedido.Cancelado)
            return BadRequest("No se puede modificar un pedido ya entregado o cancelado");

        pedido.DireccionEntrega = dto.DireccionEntrega;
        pedido.Total = dto.Total;
        pedido.Notas = dto.Notas;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // PATCH: api/pedidos/5/estado  (cambia estado y, opcionalmente, asigna repartidor)
    [HttpPatch("{id:int}/estado")]
    public async Task<IActionResult> ActualizarEstado(int id, PedidoEstadoDTO dto)
    {
        var pedido = await _context.Pedidos.FindAsync(id);

        if (pedido == null)
            return NotFound($"No se encontró el pedido con Id {id}");

        if (dto.RepartidorId.HasValue)
        {
            var repartidorExiste = await _context.Repartidores.AnyAsync(r => r.Id == dto.RepartidorId.Value);
            if (!repartidorExiste)
                return BadRequest($"No existe un repartidor con Id {dto.RepartidorId}");

            pedido.RepartidorId = dto.RepartidorId;
        }

        pedido.Estado = dto.Estado;

        if (dto.Estado == EstadoPedido.Entregado)
            pedido.FechaEntrega = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/pedidos/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> EliminarPedido(int id)
    {
        var pedido = await _context.Pedidos.FindAsync(id);

        if (pedido == null)
            return NotFound($"No se encontró el pedido con Id {id}");

        _context.Pedidos.Remove(pedido);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
