using Microsoft.AspNetCore.Mvc;
using DeliveryManagementAPI.AccesoDatos.Entidades;
using DeliveryManagementAPI.AccesoDatos.Modelos;
using DeliveryManagementAPI.AccesoDatos.Repositorio;

namespace DeliveryManagementAPI.Controladores;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly IClienteRepository _clienteRepository;

    public ClientesController(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClienteDTO>>> ObtenerTodos()
    {
        var clientes = await _clienteRepository.ObtenerTodosAsync();
        return Ok(clientes.Select(MapToDTO));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ClienteDTO>> ObtenerPorId(int id)
    {
        var cliente = await _clienteRepository.ObtenerPorIdAsync(id);
        if (cliente == null)
            return NotFound(new { mensaje = "Cliente no encontrado" });

        return Ok(MapToDTO(cliente));
    }

    [HttpGet("buscar")]
    public async Task<ActionResult<IEnumerable<ClienteDTO>>> BuscarPorNombre([FromQuery] string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            return BadRequest(new { mensaje = "El parámetro 'nombre' es requerido" });

        var clientes = await _clienteRepository.BuscarPorNombreAsync(nombre);
        return Ok(clientes.Select(MapToDTO));
    }

    [HttpPost]
    public async Task<ActionResult<ClienteDTO>> Crear([FromBody] ClienteCreateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var cliente = new ClienteEntidad
        {
            Nombre = dto.Nombre,
            Telefono = dto.Telefono,
            Direccion = dto.Direccion,
            Email = dto.Email
        };

        var clienteCreado = await _clienteRepository.CrearAsync(cliente);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = clienteCreado.Id }, MapToDTO(clienteCreado));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ClienteDTO>> Actualizar(int id, [FromBody] ClienteUpdateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var cliente = await _clienteRepository.ObtenerPorIdAsync(id);
        if (cliente == null)
            return NotFound(new { mensaje = "Cliente no encontrado" });

        if (!string.IsNullOrWhiteSpace(dto.Nombre))
            cliente.Nombre = dto.Nombre;
        if (!string.IsNullOrWhiteSpace(dto.Telefono))
            cliente.Telefono = dto.Telefono;
        if (!string.IsNullOrWhiteSpace(dto.Direccion))
            cliente.Direccion = dto.Direccion;
        if (dto.Email != null)
            cliente.Email = dto.Email;

        var clienteActualizado = await _clienteRepository.ActualizarAsync(cliente);
        return Ok(MapToDTO(clienteActualizado));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Eliminar(int id)
    {
        var resultado = await _clienteRepository.EliminarAsync(id);
        if (!resultado)
            return BadRequest(new { mensaje = "No se puede eliminar el cliente. Verifica que no tenga pedidos asociados" });

        return NoContent();
    }

    private ClienteDTO MapToDTO(ClienteEntidad cliente)
    {
        return new ClienteDTO
        {
            Id = cliente.Id,
            Nombre = cliente.Nombre,
            Telefono = cliente.Telefono,
            Direccion = cliente.Direccion,
            Email = cliente.Email,
            FechaRegistro = cliente.FechaRegistro
        };
    }
}
