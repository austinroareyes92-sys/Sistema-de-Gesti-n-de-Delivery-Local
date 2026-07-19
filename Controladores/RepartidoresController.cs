using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeliveryManagementAPI.Data;
using DeliveryManagementAPI.DTOs;
using DeliveryManagementAPI.Models;

namespace DeliveryManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RepartidoresController : ControllerBase
{
    private readonly DeliveryContext _context;

    public RepartidoresController(DeliveryContext context)
    {
        _context = context;
    }

    private static RepartidorDTO ToDTO(Repartidor r) => new()
    {
        Id = r.Id,
        Nombre = r.Nombre,
        Telefono = r.Telefono,
        Vehiculo = r.Vehiculo,
        Placa = r.Placa,
        Disponible = r.Disponible
    };

    // GET: api/repartidores
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RepartidorDTO>>> GetRepartidores()
    {
        var repartidores = await _context.Repartidores.ToListAsync();
        return Ok(repartidores.Select(ToDTO));
    }

    // GET: api/repartidores/disponibles
    [HttpGet("disponibles")]
    public async Task<ActionResult<IEnumerable<RepartidorDTO>>> GetDisponibles()
    {
        var repartidores = await _context.Repartidores
            .Where(r => r.Disponible)
            .ToListAsync();

        return Ok(repartidores.Select(ToDTO));
    }

    // GET: api/repartidores/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<RepartidorDTO>> GetRepartidor(int id)
    {
        var repartidor = await _context.Repartidores.FindAsync(id);

        if (repartidor == null)
            return NotFound($"No se encontró el repartidor con Id {id}");

        return Ok(ToDTO(repartidor));
    }

    // POST: api/repartidores
    [HttpPost]
    public async Task<ActionResult<RepartidorDTO>> CrearRepartidor(RepartidorCreateDTO dto)
    {
        var repartidor = new Repartidor
        {
            Nombre = dto.Nombre,
            Telefono = dto.Telefono,
            Vehiculo = dto.Vehiculo,
            Placa = dto.Placa,
            Disponible = true
        };

        _context.Repartidores.Add(repartidor);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetRepartidor), new { id = repartidor.Id }, ToDTO(repartidor));
    }

    // PUT: api/repartidores/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> ActualizarRepartidor(int id, RepartidorUpdateDTO dto)
    {
        var repartidor = await _context.Repartidores.FindAsync(id);

        if (repartidor == null)
            return NotFound($"No se encontró el repartidor con Id {id}");

        repartidor.Nombre = dto.Nombre;
        repartidor.Telefono = dto.Telefono;
        repartidor.Vehiculo = dto.Vehiculo;
        repartidor.Placa = dto.Placa;
        repartidor.Disponible = dto.Disponible;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/repartidores/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> EliminarRepartidor(int id)
    {
        var repartidor = await _context.Repartidores
            .Include(r => r.Pedidos)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (repartidor == null)
            return NotFound($"No se encontró el repartidor con Id {id}");

        var tienePedidosActivos = repartidor.Pedidos
            .Any(p => p.Estado != EstadoPedido.Entregado && p.Estado != EstadoPedido.Cancelado);

        if (tienePedidosActivos)
            return BadRequest("No se puede eliminar un repartidor con pedidos activos asignados");

        _context.Repartidores.Remove(repartidor);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
