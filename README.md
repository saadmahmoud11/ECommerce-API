# ECommerce API

A cleanly layered e-commerce Web API built with .NET 8 and C# 12 providing basket, payment and order workflows with Stripe payment integration.

Repository: https://github.com/saadmahmoud11/ECommerce-API

## Overview
This project implements an e-commerce backend using a domain + service layered architecture. Key responsibilities include basket management, payment intent creation/updating with Stripe, and order processing. The codebase separates contracts, entities, DTOs, service abstractions and concrete services (for example `PaymentService.cs`, `OrderService.cs`).

## Key features
- Basket CRUD and TTL-backed storage via `IBasketRepository` and `CustomerBasket`.
- Payment intent creation and updates using Stripe via `PaymentService`.
- Delivery method pricing applied to basket totals.
- Mapping between domain entities and DTOs using `AutoMapper`.
- Repository and Unit-of-Work patterns (`IGenericRepository<T, TKey>`, `IUnitOfWork`).
- Consistent result/error modeling using `Result<T>` and `Error` types for API responses.
- Config-driven secrets and settings (example key: `StripeOption:SecretKey` in `appsettings.Development.json`).

## Project structure (high level)
- `ECommerece.Domain` — domain entities and contracts (entities such as `Product`, `DeliveryMethod`, `Order`, `CustomerBasket`).
- `ECommerece.Service` — business services (for example `PaymentService.cs`, `OrderService.cs`).
- `ECommerece.ServiceAbstraction` — service interfaces (`IPaymentService`, `ICashService`, etc.).
- `ECommerece.Shared` — DTOs and shared models (`BasketDto`, `BasketItemDto`, `Result<T>`).
- `ECommerceWeb` — web host/project configuration (startup, `appsettings*.json`).

## Configuration
- Add Stripe secret in `appsettings.Development.json` or environment variable:
  - `StripeOption:SecretKey`
- Typical appsettings section:
  - `StripeOption:SecretKey: "sk_test_..."`

## How to run (local)
1. Clone repository:
   - `git clone https://github.com/saadmahmoud11/ECommerce-API`
2. Open the solution in Visual Studio 2022.
3. Update configuration values (Stripe secret, connection strings) in `appsettings.Development.json` or environment variables.
4. Build and run the `ECommerceWeb` project (set as startup).
5. Use API endpoints (or Swagger if enabled) to exercise basket and payment flows.

## Example flow (payment)
1. Client creates/updates a `CustomerBasket`.
2. Call `IPaymentService.CreatePaymentIntentAsync(basketId)`.
3. Service validates basket, refreshes item prices from `Product` repository, calculates shipping via `DeliveryMethod`.
4. `PaymentService` creates or updates a Stripe `PaymentIntent` and stores `PaymentIntentId` and `ClientSecret` on the basket.
5. Client uses `ClientSecret` to complete card payment on front-end.

## Skills & Technologies Used
- .NET 8 / C# 12
- ASP.NET Core Web API
- Stripe.NET (Stripe integration)
- AutoMapper
- Dependency Injection (Microsoft.Extensions.DependencyInjection)
- Configuration (Microsoft.Extensions.Configuration)
- Repository pattern (`IGenericRepository<T, TKey>`)
- Unit of Work pattern (`IUnitOfWork`)
- DTO design and mapping (`BasketDto`, `BasketItemDto`)
- Result / Error handling pattern (`Result<T>`, `Error`)
- Layered/Clean architecture principles
- Git / GitHub (source control)
- Visual Studio 2022
- (Commonly used but verify in solution) Entity Framework Core / relational DB and migrations
- JSON configuration (`appsettings.Development.json`)

## Project description (short)
This project is a backend API for an e-commerce platform that handles product pricing, baskets, delivery cost calculation and secure payment intent management using Stripe. It is designed with a clear separation of concerns: domain entities, repository abstractions, and service implementations. The API returns consistent `Result<T>` responses and is configuration-driven to allow safe secret management and environment-specific settings.

## Notes
- Review `appsettings.Development.json` to ensure the `StripeOption:SecretKey` is set before attempting payments.
- The `PaymentService` relies on up-to-date product pricing from the product repository to avoid stale prices when creating payment intents.
