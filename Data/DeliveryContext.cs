using Microsoft.EntityFrameworkCore;
using DeliveryManagementAPI.Models;

namespace DeliveryManagementAPI.Data;

public class DeliveryContext : DbContext
{
    public DeliveryContext(DbContextOptions<DeliveryContext> options) : base(options) { }

    public DbSet<Cliente> Clientes { get; set; } = null!;
    public DbSet<Repartidor> Repartidores { get; set; } = null!;
    public DbSet<Pedido> Pedidos { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Relación Cliente 1 --- N Pedido.
        // Restrict: no se permite borrar un cliente que tenga pedidos (se valida también en el controller).
        modelBuilder.Entity<Pedido>()
            .HasOne(p => p.Cliente)
            .WithMany(c => c.Pedidos)
            .HasForeignKey(p => p.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relación Repartidor 1 --- N Pedido (opcional).
        // SetNull: si se borra el repartidor, el pedido queda sin asignar en lugar de bloquear el borrado.
        modelBuilder.Entity<Pedido>()
            .HasOne(p => p.Repartidor)
            .WithMany(r => r.Pedidos)
            .HasForeignKey(p => p.RepartidorId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Cliente>()
            .HasIndex(c => c.Email);

        // Datos semilla para poder probar la API de inmediato
        modelBuilder.Entity<Cliente>().HasData(
            new Cliente
            {
                Id = 1,
                Nombre = "Juana Pérez",
                Telefono = "8091234567",
                Direccion = "Calle Principal #12, Higüey",
                Email = "juana.perez@example.com",
                FechaRegistro = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );

        modelBuilder.Entity<Repartidor>().HasData(
            new Repartidor
            {
                Id = 1,
                Nombre = "Carlos Gómez",
                Telefono = "8097654321",
                Vehiculo = "Moto",
                Placa = "A123456",
                Disponible = true
            }
        );
    }
}
