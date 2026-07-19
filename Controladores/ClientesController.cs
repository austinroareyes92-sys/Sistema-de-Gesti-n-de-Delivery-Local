using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeliveryManagementAPI.Data;
using DeliveryManagementAPI.DTOs;
using DeliveryManagementAPI.Models;

namespace DeliveryManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly DeliveryContext _context;

    public ClientesController(DeliveryContext context)
    {
        _context = context;
    }

    private static ClienteDTO ToDTO(Cliente c) => new()
    {
        Id = c.Id,
        Nombre = c.Nombre,
        Telefono = c.Telefono,
        Direccion = c.Direccion,
        Email = c.Email,
        FechaRegistro = c.FechaRegistro
    };

    // GET: api/clientes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClienteDTO>>> GetClientes()
    {
        var clientes = await _context.Clientes.ToListAsync();
        return Ok(clientes.Select(ToDTO));
    }

    // GET: api/clientes/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ClienteDTO>> GetCliente(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);

        if (cliente == null)
            return NotFound($"No se encontró el cliente con Id {id}");

        return Ok(ToDTO(cliente));
    }

    // POST: api/clientes
    [HttpPost]
    public async Task<ActionResult<ClienteDTO>> CrearCliente(ClienteCreateDTO dto)
    {
        var cliente = new Cliente
        {
            Nombre = dto.Nombre,
            Telefono = dto.Telefono,
            Direccion = dto.Direccion,
            Email = dto.Email,
            FechaRegistro = DateTime.UtcNow
        };

        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, ToDTO(cliente));
    }

    // PUT: api/clientes/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> ActualizarCliente(int id, ClienteUpdateDTO dto)
    {
        var cliente = await _context.Clientes.FindAsync(id);

        if (cliente == null)
            return NotFound($"No se encontró el cliente con Id {id}");

        cliente.Nombre = dto.Nombre;
        cliente.Telefono = dto.Telefono;
        cliente.Direccion = dto.Direccion;
        cliente.Email = dto.Email;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/clientes/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> EliminarCliente(int id)
    {
        var cliente = await _context.Clientes
            .Include(c => c.Pedidos)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (cliente == null)
            return NotFound($"No se encontró el cliente con Id {id}");

        if (cliente.Pedidos.Any())
            return BadRequest("No se puede eliminar un cliente que tiene pedidos registrados");

        _context.Clientes.Remove(cliente);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
