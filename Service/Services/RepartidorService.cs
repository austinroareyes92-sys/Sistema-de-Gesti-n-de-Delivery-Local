using DeliveryManagementAPI.AccesoDatos.Entidades;
using DeliveryManagementAPI.AccesoDatos.Repositorio;
using DeliveryManagementAPI.Service.Contract;
using DeliveryManagementAPI.Service.Exceptions;

namespace DeliveryManagementAPI.Service.Services;

public class RepartidorService : IRepartidorService
{
    private readonly IRepartidorRepository _repartidorRepository;

    public RepartidorService(IRepartidorRepository repartidorRepository)
    {
        _repartidorRepository = repartidorRepository;
    }

    public async Task<IEnumerable<RepartidorEntidad>> ObtenerTodosAsync()
    {
        try
        {
            return await _repartidorRepository.ObtenerTodosAsync();
        }
        catch (Exception ex)
        {
            throw new RepartidorServiceException("Error al obtener todos los repartidores", ex);
        }
    }

    public async Task<RepartidorEntidad?> ObtenerPorIdAsync(int id)
    {
        try
        {
            if (id <= 0)
                throw new ArgumentException("El ID del repartidor debe ser mayor a 0");

            return await _repartidorRepository.ObtenerPorIdAsync(id);
        }
        catch (ArgumentException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new RepartidorServiceException($"Error al obtener el repartidor con ID {id}", ex);
        }
    }

    public async Task<IEnumerable<RepartidorEntidad>> ObtenerDisponiblesAsync()
    {
        try
        {
            return await _repartidorRepository.ObtenerDisponiblesAsync();
        }
        catch (Exception ex)
        {
            throw new RepartidorServiceException("Error al obtener repartidores disponibles", ex);
        }
    }

    public async Task<RepartidorEntidad> CrearAsync(RepartidorEntidad repartidor)
    {
        try
        {
            ValidarRepartidor(repartidor);

            repartidor.Disponible = true;
            return await _repartidorRepository.CrearAsync(repartidor);
        }
        catch (RepartidorServiceException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new RepartidorServiceException("Error al crear el repartidor", ex);
        }
    }

    public async Task<RepartidorEntidad> ActualizarAsync(int id, RepartidorEntidad repartidor)
    {
        try
        {
            if (id <= 0)
                throw new ArgumentException("El ID del repartidor debe ser mayor a 0");

            var repartidorExistente = await _repartidorRepository.ObtenerPorIdAsync(id);
            if (repartidorExistente == null)
                throw new RepartidorServiceException($"El repartidor con ID {id} no existe");

            ValidarRepartidor(repartidor);

            repartidorExistente.Nombre = repartidor.Nombre;
            repartidorExistente.Telefono = repartidor.Telefono;
            repartidorExistente.Vehiculo = repartidor.Vehiculo;
            repartidorExistente.Placa = repartidor.Placa;

            return await _repartidorRepository.ActualizarAsync(repartidorExistente);
        }
        catch (RepartidorServiceException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new RepartidorServiceException($"Error al actualizar el repartidor con ID {id}", ex);
        }
    }

    public async Task<bool> EliminarAsync(int id)
    {
        try
        {
            if (id <= 0)
                throw new ArgumentException("El ID del repartidor debe ser mayor a 0");

            var repartidor = await _repartidorRepository.ObtenerPorIdAsync(id);
            if (repartidor == null)
                throw new RepartidorServiceException($"El repartidor con ID {id} no existe");

            return await _repartidorRepository.EliminarAsync(id);
        }
        catch (RepartidorServiceException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new RepartidorServiceException($"Error al eliminar el repartidor con ID {id}", ex);
        }
    }

    public async Task<bool> CambiarDisponibilidadAsync(int id, bool disponible)
    {
        try
        {
            if (id <= 0)
                throw new ArgumentException("El ID del repartidor debe ser mayor a 0");

            var repartidor = await _repartidorRepository.ObtenerPorIdAsync(id);
            if (repartidor == null)
                throw new RepartidorServiceException($"El repartidor con ID {id} no existe");

            repartidor.Disponible = disponible;
            await _repartidorRepository.ActualizarAsync(repartidor);
            return true;
        }
        catch (RepartidorServiceException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new RepartidorServiceException($"Error al cambiar la disponibilidad del repartidor con ID {id}", ex);
        }
    }

    private void ValidarRepartidor(RepartidorEntidad repartidor)
    {
        if (repartidor == null)
            throw new ArgumentNullException(nameof(repartidor));

        if (string.IsNullOrWhiteSpace(repartidor.Nombre))
            throw new ArgumentException("El nombre del repartidor es obligatorio");

        if (repartidor.Nombre.Length < 3 || repartidor.Nombre.Length > 100)
            throw new ArgumentException("El nombre debe tener entre 3 y 100 caracteres");

        if (string.IsNullOrWhiteSpace(repartidor.Telefono))
            throw new ArgumentException("El teléfono del repartidor es obligatorio");

        if (!System.Text.RegularExpressions.Regex.IsMatch(repartidor.Telefono, @"^[0-9\-\+\(\)\s]{7,20}$"))
            throw new ArgumentException("El teléfono tiene un formato inválido");

        if (string.IsNullOrWhiteSpace(repartidor.Vehiculo))
            throw new ArgumentException("El tipo de vehículo es obligatorio");

        if (repartidor.Vehiculo.Length < 2 || repartidor.Vehiculo.Length > 50)
            throw new ArgumentException("El vehículo debe tener entre 2 y 50 caracteres");

        if (!string.IsNullOrWhiteSpace(repartidor.Placa))
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(repartidor.Placa, @"^[A-Z0-9]{6,15}$"))
                throw new ArgumentException("La placa tiene un formato inválido (debe ser alfanumérica)");
        }
    }
}
