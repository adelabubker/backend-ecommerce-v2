# Kosa ECommerce Backend

Clean-architecture backend for the Kosa e-commerce platform, migrated and modernized with full feature parity taken from the Cayan.Jannah reference implementation.

## Layers

- `Kosa.ECommerce.Server` — ASP.NET Core Web API (controllers, JWT auth, global exception middleware, OpenAPI).
- `Kosa.ECommerce.Application` — DTOs, service contracts, business logic, AutoMapper profile, DI.
- `Kosa.ECommerce.Persistence` — EF Core, `KosaDbContext`, entities, repositories, DI.
- `Kosa.ECommerce.Infrastructure` — JWT token generator, PBKDF2 password hashing.
- `Kosa.ECommerce.Shared` — `Result<T>`, `PagedResult<T>`, helpers, exceptions.
- `Kosa.ECommerce.Tests` — unit tests for services, mapping and hashing.

## Framework

- .NET 10 (was .NET 8), EF Core 10, AutoMapper 16.2, JWT Bearer 10.

## API endpoints

- `GET  /api/server/health`
- `POST /api/auth/register` · `POST /api/auth/login` · `GET /api/auth/me` (auth)
- `GET/PUT /api/profile` · `GET/POST /api/profile/addresses` · `DELETE /api/profile/addresses/{id}` (auth)
- `GET /api/products` · `GET /api/products/{id}` · `GET /api/products/{slug}` · `GET /api/products/category/{categoryId}` · `GET /api/products/search?name=` 
- `GET /api/categories`
- `GET /api/promotions`
- `GET /api/cart` · `POST /api/cart/items` · `PUT /api/cart/items/quantity` · `DELETE /api/cart/items/{productId}` (auth)
- `GET/POST /api/favorites` · `DELETE /api/favorites/{favoriteId}` (auth)
- `GET/POST /api/orders` · `GET /api/orders/{orderId}` · `GET /api/orders/by-number/{orderNumber}` (auth)

## Migration / EF Core

```bash
cd backend
dotnet tool restore
dotnet ef migrations add InitialCreate --project src/Kosa.ECommerce.Persistence --startup-project src/Kosa.ECommerce.Server
dotnet ef database update --project src/Kosa.ECommerce.Persistence --startup-project src/Kosa.ECommerce.Server
```
