using System.ComponentModel.DataAnnotations;

namespace DeliveryManagementAPI.DTOs;

/// <summary>DTO de salida: lo que la API devuelve al consultar clientes.</summary>
public class ClienteDTO
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string? Email { get; set; }
    public DateTime FechaRegistro { get; set; }
}

/// <summary>DTO de entrada para crear un cliente.</summary>
public class ClienteCreateDTO
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono es obligatorio")]
    [Phone]
    [StringLength(20)]
    public string Telefono { get; set; } = string.Empty;

    [Required(ErrorMessage = "La dirección es obligatoria")]
    [StringLength(200)]
    public string Direccion { get; set; } = string.Empty;

    [EmailAddress]
    [StringLength(150)]
    public string? Email { get; set; }
}

/// <summary>DTO de entrada para actualizar un cliente existente.</summary>
public class ClienteUpdateDTO
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono es obligatorio")]
    [Phone]
    [StringLength(20)]
    public string Telefono { get; set; } = string.Empty;

    [Required(ErrorMessage = "La dirección es obligatoria")]
    [StringLength(200)]
    public string Direccion { get; set; } = string.Empty;

    [EmailAddress]
    [StringLength(150)]
    public string? Email { get; set; }
}
