using Microsoft.EntityFrameworkCore;
using DeliveryManagementAPI.AccesoDatos.Contexto;
using DeliveryManagementAPI.AccesoDatos.Entidades;

namespace DeliveryManagementAPI.AccesoDatos.Repositorio;

public class RepartidorRepository : IRepartidorRepository
{
    private readonly DeliveryContext _context;

    public RepartidorRepository(DeliveryContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RepartidorEntidad>> ObtenerTodosAsync()
    {
        return await _context.Repartidores.Include(r => r.Pedidos).ToListAsync();
    }

    public async Task<RepartidorEntidad?> ObtenerPorIdAsync(int id)
    {
        return await _context.Repartidores
            .Include(r => r.Pedidos)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<RepartidorEntidad>> ObtenerDisponiblesAsync()
    {
        return await _context.Repartidores
            .Where(r => r.Disponible)
            .Include(r => r.Pedidos)
            .ToListAsync();
    }

    public async Task<RepartidorEntidad> CrearAsync(RepartidorEntidad repartidor)
    {
        _context.Repartidores.Add(repartidor);
        await _context.SaveChangesAsync();
        return repartidor;
    }

    public async Task<RepartidorEntidad> ActualizarAsync(RepartidorEntidad repartidor)
    {
        _context.Repartidores.Update(repartidor);
        await _context.SaveChangesAsync();
        return repartidor;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var repartidor = await ObtenerPorIdAsync(id);
        if (repartidor == null) return false;

        _context.Repartidores.Remove(repartidor);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> GuardarCambiosAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
