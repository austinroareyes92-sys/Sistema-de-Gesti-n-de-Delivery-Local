using Microsoft.EntityFrameworkCore;
using DeliveryManagementAPI.AccesoDatos.Contexto;
using DeliveryManagementAPI.AccesoDatos.Entidades;
using DeliveryManagementAPI.Models;

namespace DeliveryManagementAPI.AccesoDatos.Repositorio;

public class PedidoRepository : IPedidoRepository
{
    private readonly DeliveryContext _context;

    public PedidoRepository(DeliveryContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PedidoEntidad>> ObtenerTodosAsync()
    {
        return await _context.Pedidos
            .Include(p => p.Cliente)
            .Include(p => p.Repartidor)
            .ToListAsync();
    }

    public async Task<IEnumerable<PedidoEntidad>> ObtenerPorEstadoAsync(EstadoPedido estado)
    {
        return await _context.Pedidos
            .Where(p => p.Estado == estado)
            .Include(p => p.Cliente)
            .Include(p => p.Repartidor)
            .ToListAsync();
    }

    public async Task<PedidoEntidad?> ObtenerPorIdAsync(int id)
    {
        return await _context.Pedidos
            .Include(p => p.Cliente)
            .Include(p => p.Repartidor)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<PedidoEntidad>> ObtenerPorClienteAsync(int clienteId)
    {
        return await _context.Pedidos
            .Where(p => p.ClienteId == clienteId)
            .Include(p => p.Cliente)
            .Include(p => p.Repartidor)
            .ToListAsync();
    }

    public async Task<IEnumerable<PedidoEntidad>> ObtenerPorRepartidorAsync(int repartidorId)
    {
        return await _context.Pedidos
            .Where(p => p.RepartidorId == repartidorId)
            .Include(p => p.Cliente)
            .Include(p => p.Repartidor)
            .ToListAsync();
    }

    public async Task<PedidoEntidad> CrearAsync(PedidoEntidad pedido)
    {
        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();
        return pedido;
    }

    public async Task<PedidoEntidad> ActualizarAsync(PedidoEntidad pedido)
    {
        _context.Pedidos.Update(pedido);
        await _context.SaveChangesAsync();
        return pedido;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var pedido = await ObtenerPorIdAsync(id);
        if (pedido == null) return false;

        _context.Pedidos.Remove(pedido);
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
