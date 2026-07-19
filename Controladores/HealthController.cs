using Microsoft.AspNetCore.Mvc;
using DeliveryManagementAPI.AccesoDatos.Contexto;
using DeliveryManagementAPI.AccesoDatos.Modelos;

namespace DeliveryManagementAPI.Controladores;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly DeliveryContext _context;

    public HealthController(DeliveryContext context)
    {
        _context = context;
    }

    [HttpGet("check")]
    public async Task<ActionResult<HealthCheckResponse>> HealthCheck()
    {
        try
        {
            // Intentar conectar a la base de datos
            var canConnect = await _context.Database.CanConnectAsync();
            
            if (canConnect)
            {
                return Ok(new HealthCheckResponse
                {
                    Status = "ok",
                    Database = "connected",
                    Timestamp = DateTime.UtcNow
                });
            }
            else
            {
                return StatusCode(503, new HealthCheckResponse
                {
                    Status = "error",
                    Database = "disconnected",
                    Timestamp = DateTime.UtcNow
                });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(503, new HealthCheckResponse
            {
                Status = "error",
                Database = $"error: {ex.Message}",
                Timestamp = DateTime.UtcNow
            });
        }
    }
}
