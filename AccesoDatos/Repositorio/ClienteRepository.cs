using Microsoft.EntityFrameworkCore;
using DeliveryManagementAPI.AccesoDatos.Contexto;
using DeliveryManagementAPI.AccesoDatos.Entidades;

namespace DeliveryManagementAPI.AccesoDatos.Repositorio;

public class ClienteRepository : IClienteRepository
{
    private readonly DeliveryContext _context;

    public ClienteRepository(DeliveryContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ClienteEntidad>> ObtenerTodosAsync()
    {
        return await _context.Clientes.Include(c => c.Pedidos).ToListAsync();
    }

    public async Task<ClienteEntidad?> ObtenerPorIdAsync(int id)
    {
        return await _context.Clientes
            .Include(c => c.Pedidos)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<ClienteEntidad>> BuscarPorNombreAsync(string nombre)
    {
        return await _context.Clientes
            .Where(c => c.Nombre.ToLower().Contains(nombre.ToLower()))
            .Include(c => c.Pedidos)
            .ToListAsync();
    }

    public async Task<ClienteEntidad> CrearAsync(ClienteEntidad cliente)
    {
        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();
        return cliente;
    }

    public async Task<ClienteEntidad> ActualizarAsync(ClienteEntidad cliente)
    {
        _context.Clientes.Update(cliente);
        await _context.SaveChangesAsync();
        return cliente;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var cliente = await ObtenerPorIdAsync(id);
        if (cliente == null) return false;

        if (cliente.Pedidos.Any())
            return false; // No se puede eliminar si tiene pedidos

        _context.Clientes.Remove(cliente);
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
