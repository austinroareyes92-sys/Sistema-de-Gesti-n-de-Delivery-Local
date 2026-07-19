using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using DeliveryManagementAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Controllers (serializa los enums como texto, ej. "Pendiente" en vez de 0)
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// DbContext - SQL Server, cadena de conexión desde appsettings.json
builder.Services.AddDbContext<DeliveryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

if (app.Environment.IsDevelopment())
{
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
