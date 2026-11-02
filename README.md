# AQPlunch

Plataforma de gestión para restaurantes: menús, mesas con código QR, pedidos y ventas. Usa una arquitectura orientada a servicios (SOA) con .NET 10, Entity Framework Core y PostgreSQL.

## Servicios

| Servicio | Carpeta | Puerto | Base de datos | Responsable |
| --- | --- | --- | --- | --- |
| IdentityService | `Identity/` | 5001 | `aqplunch_identity` | Valeria |
| RestaurantService | `Restaurant/` | 5002 | `aqplunch_restaurant` | Miler |
| OrderService | `Order/` | 5003 | `aqplunch_order` | Jeremy |
| SalesService | `Sales/` | 5004 | `aqplunch_sales` | Jose |
| Gateway (YARP) | `Gateway/` | 5000 | — | Jeremy |

## Estructura de cada servicio

Cada servicio tiene 4 proyectos con arquitectura hexagonal y CQRS con MediatR:

- `Api`: controladores, filtro de excepciones, JWT y Swagger.
- `Application`: commands, queries y sus handlers.
- `Domain`: entidades, interfaces de repositorios y excepciones.
- `Infrastructure`: DbContext, repositorios y servicios técnicos.

## Cómo levantar el proyecto en tu PC

### Lo que necesitas

- .NET 10 SDK y Rider.
- PostgreSQL instalado y encendido, con la contraseña de tu usuario `postgres`.
- La clave del JWT del grupo. Se comparte por WhatsApp y no se sube al repositorio.

### Pasos

1. Clona el repositorio o haz **Pull** de `main`.
2. En la carpeta del proyecto, haz clic derecho en `instalar.ps1` y elige **Ejecutar con PowerShell**. También se puede ejecutar desde una terminal:

   ```powershell
   powershell -ExecutionPolicy Bypass -File .\instalar.ps1
   ```

3. Escribe la contraseña de tu PostgreSQL y la clave del JWT cuando el script las pida.
4. Abre `AQPlunch.sln` en Rider, elige el servicio en el selector (por ejemplo `AQPlunch.Identity.Api: http`) y dale **Play**.
5. Abre el Swagger del servicio en su puerto, por ejemplo `http://localhost:5001`.

### Qué hace `instalar.ps1`

- Crea las bases `aqplunch_identity`, `aqplunch_restaurant`, `aqplunch_order` y `aqplunch_sales` si no existen.
- Ejecuta el script de tablas y el de datos de prueba de cada servicio, solo si la base está vacía. Una base que ya tiene tablas no se toca.
- Genera el `appsettings.Development.json` de cada servicio con tu contraseña y la clave del JWT. Ese archivo no se sube al repositorio; `appsettings.json` solo tiene valores de ejemplo.

Se puede volver a ejecutar cuando se agregue un servicio nuevo. Para empezar una base de cero, bórrala en pgAdmin y ejecuta el script otra vez.

### Usuarios de prueba

| Usuario | Contraseña | Rol |
| --- | --- | --- |
| `admin` | `Admin123*` | Admin |
| `restaurante1` | `Restaurante123*` | Restaurante (Picantería Los Volcanes) |
| `restaurante2` | `Restaurante123*` | Restaurante (Cevichería Mar Azul) |

## Acuerdos entre servicios

### Roles

- `Admin`: administra la plataforma.
- `Restaurante`: personal de un restaurante.

El cliente pide por el QR de su mesa, sin cuenta ni rol.

### JWT

Lo emite IdentityService y los demás servicios lo validan con los mismos valores:

| Valor | Acuerdo |
| --- | --- |
| Issuer | `AQPlunch.Identity` |
| Audience | `AQPlunch` |
| Clave | La misma en los 4 servicios, en `Jwt:SecretKey` del `appsettings.Development.json`. Se comparte por WhatsApp y no se sube |
| Claims | `userId`, `role` y `restaurantId` (solo para el rol Restaurante) |

### Rutas que se consumen entre servicios

| Quién la usa | Servicio | Ruta | Devuelve |
| --- | --- | --- | --- |
| Order | Restaurant | `GET api/mesas/token/{token}` (pública) | Mesa, número y restaurante |
| Order | Restaurant | `GET api/productos/por-ids?ids=…` | Id, nombre, precio, disponible y restaurante de cada producto |
| Sales | Order | `GET api/pedidos/{id}` | Pedido con su detalle, estado y total |

### Prefijos de rutas por servicio

Cada servicio usa prefijos propios para que el Gateway sepa a dónde enviar cada petición.

| Servicio | Prefijos |
| --- | --- |
| Identity | `api/auth`, `api/usuarios`, `api/roles` |
| Restaurant | `api/restaurantes`, `api/mesas`, `api/categorias`, `api/productos` |
| Order | `api/pedidos` |
| Sales | `api/ventas`, `api/reportes` |
