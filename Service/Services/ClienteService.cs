using DeliveryManagementAPI.AccesoDatos.Entidades;
using DeliveryManagementAPI.AccesoDatos.Repositorio;
using DeliveryManagementAPI.Service.Contract;
using DeliveryManagementAPI.Service.Exceptions;

namespace DeliveryManagementAPI.Service.Services;

public class ClienteService : IClienteService
{
    private readonly IClienteRepository _clienteRepository;

    public ClienteService(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task<IEnumerable<ClienteEntidad>> ObtenerTodosAsync()
    {
        try
        {
            return await _clienteRepository.ObtenerTodosAsync();
        }
        catch (Exception ex)
        {
            throw new ClienteServiceException("Error al obtener todos los clientes", ex);
        }
    }

    public async Task<ClienteEntidad?> ObtenerPorIdAsync(int id)
    {
        try
        {
            if (id <= 0)
                throw new ArgumentException("El ID del cliente debe ser mayor a 0");

            return await _clienteRepository.ObtenerPorIdAsync(id);
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ClienteServiceException($"Error al obtener el cliente con ID {id}", ex);
        }
    }

    public async Task<IEnumerable<ClienteEntidad>> BuscarPorNombreAsync(string nombre)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre del cliente es requerido para la búsqueda");

            if (nombre.Length < 3)
                throw new ArgumentException("El nombre debe tener al menos 3 caracteres");

            return await _clienteRepository.BuscarPorNombreAsync(nombre);
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ClienteServiceException($"Error al buscar clientes por nombre: {nombre}", ex);
        }
    }

    public async Task<ClienteEntidad> CrearAsync(ClienteEntidad cliente)
    {
        try
        {
            ValidarCliente(cliente);

            // Verificar que no exista un cliente con el mismo email
            if (!string.IsNullOrWhiteSpace(cliente.Email))
            {
                var clientes = await _clienteRepository.ObtenerTodosAsync();
                if (clientes.Any(c => c.Email == cliente.Email))
                    throw new ClienteServiceException($"Ya existe un cliente registrado con el email {cliente.Email}");
            }

            cliente.FechaRegistro = DateTime.UtcNow;
            return await _clienteRepository.CrearAsync(cliente);
        }
        catch (ClienteServiceException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ClienteServiceException("Error al crear el cliente", ex);
        }
    }

    public async Task<ClienteEntidad> ActualizarAsync(int id, ClienteEntidad cliente)
    {
        try
        {
            if (id <= 0)
                throw new ArgumentException("El ID del cliente debe ser mayor a 0");

            var clienteExistente = await _clienteRepository.ObtenerPorIdAsync(id);
            if (clienteExistente == null)
                throw new ClienteServiceException($"El cliente con ID {id} no existe");

            ValidarCliente(cliente);

            // Verificar email único si se está actualizando
            if (!string.IsNullOrWhiteSpace(cliente.Email) && cliente.Email != clienteExistente.Email)
            {
                var clientes = await _clienteRepository.ObtenerTodosAsync();
                if (clientes.Any(c => c.Email == cliente.Email && c.Id != id))
                    throw new ClienteServiceException($"Ya existe otro cliente registrado con el email {cliente.Email}");
            }

            clienteExistente.Nombre = cliente.Nombre;
            clienteExistente.Telefono = cliente.Telefono;
            clienteExistente.Direccion = cliente.Direccion;
            clienteExistente.Email = cliente.Email;

            return await _clienteRepository.ActualizarAsync(clienteExistente);
        }
        catch (ClienteServiceException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ClienteServiceException($"Error al actualizar el cliente con ID {id}", ex);
        }
    }

    public async Task<bool> EliminarAsync(int id)
    {
        try
        {
            if (id <= 0)
                throw new ArgumentException("El ID del cliente debe ser mayor a 0");

            var cliente = await _clienteRepository.ObtenerPorIdAsync(id);
            if (cliente == null)
                throw new ClienteServiceException($"El cliente con ID {id} no existe");

            return await _clienteRepository.EliminarAsync(id);
        }
        catch (ClienteServiceException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ClienteServiceException($"Error al eliminar el cliente con ID {id}", ex);
        }
    }

    private void ValidarCliente(ClienteEntidad cliente)
    {
        if (cliente == null)
            throw new ArgumentNullException(nameof(cliente));

        if (string.IsNullOrWhiteSpace(cliente.Nombre))
            throw new ArgumentException("El nombre del cliente es obligatorio");

        if (cliente.Nombre.Length < 3 || cliente.Nombre.Length > 100)
            throw new ArgumentException("El nombre debe tener entre 3 y 100 caracteres");

        if (string.IsNullOrWhiteSpace(cliente.Telefono))
            throw new ArgumentException("El teléfono del cliente es obligatorio");

        if (!System.Text.RegularExpressions.Regex.IsMatch(cliente.Telefono, @"^[0-9\-\+\(\)\s]{7,20}$"))
            throw new ArgumentException("El teléfono tiene un formato inválido");

        if (string.IsNullOrWhiteSpace(cliente.Direccion))
            throw new ArgumentException("La dirección del cliente es obligatoria");

        if (cliente.Direccion.Length < 5 || cliente.Direccion.Length > 200)
            throw new ArgumentException("La dirección debe tener entre 5 y 200 caracteres");

        if (!string.IsNullOrWhiteSpace(cliente.Email))
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(cliente.Email);
                if (cliente.Email != addr.Address)
                    throw new ArgumentException("El email tiene un formato inválido");
            }
            catch
            {
                throw new ArgumentException("El email tiene un formato inválido");
            }
        }
    }
}
