using Microsoft.AspNetCore.Mvc;
using DeliveryManagementAPI.AccesoDatos.Entidades;
using DeliveryManagementAPI.AccesoDatos.Modelos;
using DeliveryManagementAPI.AccesoDatos.Repositorio;

namespace DeliveryManagementAPI.Controladores;

[ApiController]
[Route("api/[controller]")]
public class RepartidoresController : ControllerBase
{
    private readonly IRepartidorRepository _repartidorRepository;

    public RepartidoresController(IRepartidorRepository repartidorRepository)
    {
        _repartidorRepository = repartidorRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RepartidorDTO>>> ObtenerTodos()
    {
        var repartidores = await _repartidorRepository.ObtenerTodosAsync();
        return Ok(repartidores.Select(MapToDTO));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RepartidorDTO>> ObtenerPorId(int id)
    {
        var repartidor = await _repartidorRepository.ObtenerPorIdAsync(id);
        if (repartidor == null)
            return NotFound(new { mensaje = "Repartidor no encontrado" });

        return Ok(MapToDTO(repartidor));
    }

    [HttpGet("disponibles")]
    public async Task<ActionResult<IEnumerable<RepartidorDTO>>> ObtenerDisponibles()
    {
        var repartidores = await _repartidorRepository.ObtenerDisponiblesAsync();
        return Ok(repartidores.Select(MapToDTO));
    }

    [HttpPost]
    public async Task<ActionResult<RepartidorDTO>> Crear([FromBody] RepartidorCreateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var repartidor = new RepartidorEntidad
        {
            Nombre = dto.Nombre,
            Telefono = dto.Telefono,
            Vehiculo = dto.Vehiculo,
            Placa = dto.Placa
        };

        var repartidorCreado = await _repartidorRepository.CrearAsync(repartidor);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = repartidorCreado.Id }, MapToDTO(repartidorCreado));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<RepartidorDTO>> Actualizar(int id, [FromBody] RepartidorUpdateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var repartidor = await _repartidorRepository.ObtenerPorIdAsync(id);
        if (repartidor == null)
            return NotFound(new { mensaje = "Repartidor no encontrado" });

        if (!string.IsNullOrWhiteSpace(dto.Nombre))
            repartidor.Nombre = dto.Nombre;
        if (!string.IsNullOrWhiteSpace(dto.Telefono))
            repartidor.Telefono = dto.Telefono;
        if (!string.IsNullOrWhiteSpace(dto.Vehiculo))
            repartidor.Vehiculo = dto.Vehiculo;
        if (dto.Placa != null)
            repartidor.Placa = dto.Placa;
        if (dto.Disponible.HasValue)
            repartidor.Disponible = dto.Disponible.Value;

        var repartidorActualizado = await _repartidorRepository.ActualizarAsync(repartidor);
        return Ok(MapToDTO(repartidorActualizado));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Eliminar(int id)
    {
        var resultado = await _repartidorRepository.EliminarAsync(id);
        if (!resultado)
            return NotFound(new { mensaje = "Repartidor no encontrado" });

        return NoContent();
    }

    private RepartidorDTO MapToDTO(RepartidorEntidad repartidor)
    {
        return new RepartidorDTO
        {
            Id = repartidor.Id,
            Nombre = repartidor.Nombre,
            Telefono = repartidor.Telefono,
            Vehiculo = repartidor.Vehiculo,
            Placa = repartidor.Placa,
            Disponible = repartidor.Disponible
        };
    }
}
