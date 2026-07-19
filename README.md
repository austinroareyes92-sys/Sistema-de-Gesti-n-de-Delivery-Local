# Delivery Management API

API REST (ASP.NET Core 8 + Entity Framework Core) para la gestión de un sistema de delivery local.

## Modelo de datos

- **Cliente** — quien realiza el pedido.
- **Repartidor** — quien lo entrega.
- **Pedido** — entidad central. Pertenece a un `Cliente` (obligatorio) y puede tener un `Repartidor` asignado (opcional).

```
Cliente (1) ────< (N) Pedido (N) >──── (1) Repartidor
```

## Requisitos previos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server o SQL Server LocalDB (incluido con Visual Studio) — o cambia el proveedor si prefieres otro motor (ver abajo)
- Herramientas de EF Core: `dotnet tool install --global dotnet-ef` (si no las tienes)

## Puesta en marcha

```bash
# 1. Restaurar paquetes NuGet
dotnet restore

# 2. Ajustar la cadena de conexión si es necesario
#    (appsettings.json -> ConnectionStrings:DefaultConnection)

# 3. Crear la migración inicial
dotnet ef migrations add InicialCreate

# 4. Aplicar la migración y crear la base de datos
dotnet ef database update

# 5. Ejecutar el proyecto
dotnet run
```

Al ejecutar en modo Development se abre Swagger automáticamente en `/swagger`, donde puedes probar todos los endpoints.

## Usar otro motor de base de datos

El proyecto usa SQL Server por defecto. Para cambiarlo:

1. Reemplaza el paquete `Microsoft.EntityFrameworkCore.SqlServer` en el `.csproj` por el de tu proveedor:
   - PostgreSQL: `Npgsql.EntityFrameworkCore.PostgreSQL`
   - MySQL: `Pomelo.EntityFrameworkCore.MySql`
   - SQLite (ideal para pruebas rápidas sin instalar un motor): `Microsoft.EntityFrameworkCore.Sqlite`
2. En `Program.cs`, cambia `options.UseSqlServer(...)` por el método equivalente (`UseNpgsql`, `UseMySql`, `UseSqlite`).
3. Actualiza la cadena de conexión en `appsettings.json`.

## Endpoints

### Clientes (`/api/clientes`)
| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/clientes` | Lista todos los clientes |
| GET | `/api/clientes/{id}` | Obtiene un cliente por Id |
| POST | `/api/clientes` | Crea un cliente |
| PUT | `/api/clientes/{id}` | Actualiza un cliente |
| DELETE | `/api/clientes/{id}` | Elimina un cliente (rechaza si tiene pedidos) |

### Repartidores (`/api/repartidores`)
| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/repartidores` | Lista todos los repartidores |
| GET | `/api/repartidores/disponibles` | Lista solo los repartidores disponibles |
| GET | `/api/repartidores/{id}` | Obtiene un repartidor por Id |
| POST | `/api/repartidores` | Crea un repartidor |
| PUT | `/api/repartidores/{id}` | Actualiza un repartidor |
| DELETE | `/api/repartidores/{id}` | Elimina un repartidor (rechaza si tiene pedidos activos) |

### Pedidos (`/api/pedidos`)
| Método | Ruta | Descripción |
|---|---|---|
| GET | `/api/pedidos` | Lista todos los pedidos (con nombre de cliente/repartidor) |
| GET | `/api/pedidos/{id}` | Obtiene un pedido por Id |
| GET | `/api/pedidos/cliente/{clienteId}` | Lista los pedidos de un cliente |
| POST | `/api/pedidos` | Crea un pedido (estado inicial: Pendiente) |
| PUT | `/api/pedidos/{id}` | Actualiza dirección/total/notas (no aplica si ya fue entregado/cancelado) |
| PATCH | `/api/pedidos/{id}/estado` | Cambia el estado y opcionalmente asigna repartidor |
| DELETE | `/api/pedidos/{id}` | Elimina un pedido |

Estados posibles de un pedido: `Pendiente`, `EnPreparacion`, `EnCamino`, `Entregado`, `Cancelado`.

## Ejemplo: crear un pedido y asignarlo

```bash
# Crear pedido
curl -X POST http://localhost:5136/api/pedidos \
  -H "Content-Type: application/json" \
  -d '{"clienteId":1,"direccionEntrega":"Av. Libertad #45","total":850.00,"notas":"Tocar el timbre"}'

# Asignar repartidor y marcar en camino
curl -X PATCH http://localhost:5136/api/pedidos/1/estado \
  -H "Content-Type: application/json" \
  -d '{"estado":"EnCamino","repartidorId":1}'
```

## Estructura del proyecto

```
DeliveryManagementAPI/
├── Controllers/       # Endpoints HTTP (ClientesController, RepartidoresController, PedidosController)
├── Models/             # Entidades de EF Core (Cliente, Repartidor, Pedido, EstadoPedido)
├── DTOs/                # Objetos de entrada/salida de la API, separados de las entidades
├── Data/                # DeliveryContext (DbContext) y configuración de relaciones
├── Program.cs           # Configuración de servicios, DbContext, Swagger, CORS
├── appsettings.json      # Cadena de conexión y configuración
└── Properties/launchSettings.json
```
