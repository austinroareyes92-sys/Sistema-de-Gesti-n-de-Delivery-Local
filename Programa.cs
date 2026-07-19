using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using DeliveryManagementAPI.AccesoDatos.Contexto;
using DeliveryManagementAPI.AccesoDatos.Repositorio;
using DeliveryManagementAPI.Service.Contract;
using DeliveryManagementAPI.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Controllers (serializa los enums como texto, ej. "Pendiente" en vez de 0)
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// DbContext - SQLite para mayor portabilidad
builder.Services.AddDbContext<DeliveryContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=delivery.db"));

// Inyección de Dependencias - Repositorios
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IRepartidorRepository, RepartidorRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();

// Inyección de Dependencias - Servicios de Negocio
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IRepartidorService, RepartidorService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Delivery Management API",
        Version = "v1",
        Description = "API REST para la gestión de clientes, repartidores y pedidos de un sistema de delivery local"
    });
});

// CORS abierto para desarrollo (ajustar dominios permitidos en producción)
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Aplicar migraciones automáticamente en desarrollo
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<DeliveryContext>();
        context.Database.EnsureCreated();
    }

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Delivery Management API v1");
    });
}

app.UseHttpsRedirection();
app.UseCors("PermitirTodo");
app.UseAuthorization();
app.MapControllers();

app.Run();
