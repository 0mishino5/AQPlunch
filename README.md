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

## Configuración local

`appsettings.json` solo tiene valores de ejemplo. Cada persona crea un `appsettings.Development.json` en el proyecto Api de su servicio, con su contraseña de PostgreSQL y la clave del JWT. Ese archivo no se sube al repositorio.
