using Microsoft.EntityFrameworkCore;
using DeliveryManagementAPI.AccesoDatos.Entidades;

namespace DeliveryManagementAPI.AccesoDatos.Contexto;

public class DeliveryContext : DbContext
{
    public DeliveryContext(DbContextOptions<DeliveryContext> options) : base(options) { }

    public DbSet<ClienteEntidad> Clientes { get; set; } = null!;
    public DbSet<RepartidorEntidad> Repartidores { get; set; } = null!;
    public DbSet<PedidoEntidad> Pedidos { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Relación Cliente 1 --- N Pedido.
        // Restrict: no se permite borrar un cliente que tenga pedidos.
        modelBuilder.Entity<PedidoEntidad>()
            .HasOne(p => p.Cliente)
            .WithMany(c => c.Pedidos)
            .HasForeignKey(p => p.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relación Repartidor 1 --- N Pedido (opcional).
        // SetNull: si se borra el repartidor, el pedido queda sin asignar.
        modelBuilder.Entity<PedidoEntidad>()
            .HasOne(p => p.Repartidor)
            .WithMany(r => r.Pedidos)
            .HasForeignKey(p => p.RepartidorId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<ClienteEntidad>()
            .HasIndex(c => c.Email);

        // Datos semilla
        modelBuilder.Entity<ClienteEntidad>().HasData(
            new ClienteEntidad
            {
                Id = 1,
                Nombre = "Juana Pérez",
                Telefono = "8091234567",
                Direccion = "Calle Principal #12, Higüey",
                Email = "juana.perez@example.com",
                FechaRegistro = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );

        modelBuilder.Entity<RepartidorEntidad>().HasData(
            new RepartidorEntidad
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
