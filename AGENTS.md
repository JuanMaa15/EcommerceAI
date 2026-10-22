## EcommerceAI 
- Este proyecto consiste en una API REST para un ecommerce con funcionalidades de inteligencia artificial.


## ==================================  API BACKEND  =================================

## Stack 
- Lenguaje: C# .NET
- Framework / runtime: .Net, ASP.NET Core 
- Base de datos: SQL Server (Principal)  y PostgreSQL (Vectorial) 
- Tests: (Por definir)
- Validaciones: (Por definir)

## Comandos
- Por definir

## Buenas practicas
- Usar prinicipios de codigo limpio como SOLID, DRY, KISS, YAGNI segun sea necesario.


## Estructura del proyecto 
# Arquitectura del proyecto

Este proyecto utiliza una arquitectura Clean Architecture tradicional.

El proyecto está dividido en las siguientes capas:

- Application
- Domain
- Infrastructure
- WebApi

## Application

La capa `Application` contiene la lógica de aplicación y los casos de uso del sistema.

Estructura:

- `DTOs/` — Objetos utilizados para transferir datos entre la API y la capa de aplicación.
- `UseCases/` — Casos de uso de la aplicación. Cada caso de uso representa una acción u operación que el sistema puede realizar.
- `Validators/` — Reglas de validación para los DTOs y las entradas de los casos de uso.

La capa Application no debe depender de Infrastructure.

## Domain

La capa `Domain` contiene el modelo central del negocio y sus reglas de negocio.

Estructura:

- `Entities/` — Entidades del dominio y su comportamiento relacionado con el negocio.
- `Repositories/` — Contratos e interfaces de los repositorios requeridos por el dominio.
- `Enums/` — Enumeraciones pertenecientes al dominio.
- `Interfaces/` — Otros contratos y abstracciones necesarios para el dominio.

La capa Domain debe mantenerse independiente de Infrastructure, bases de datos, frameworks, servicios externos y detalles de implementación.

## Infrastructure

La capa `Infrastructure` contiene las implementaciones relacionadas con persistencia, bases de datos y servicios externos.

Estructura:

- `Persistence/` — Componentes relacionados con la persistencia de datos.
  - `Configurations/` — Configuraciones de las entidades de Entity Framework Core.
  - `ApplicationDbContext` — Contexto de base de datos de Entity Framework Core.
  - `Repositories/` — Implementaciones concretas de los repositorios definidos mediante interfaces en las capas internas.
  - `Migrations/` — Migraciones de Entity Framework Core.
- `Services/` — Implementaciones de servicios externos e integraciones con APIs de terceros.
- `DependencyInjection/` — Configuración y registro de las dependencias de Infrastructure.

Infrastructure implementa las abstracciones definidas por las capas internas.

Los servicios externos, como proveedores de correo electrónico, servicios de pago, APIs de terceros u otras integraciones externas, deben implementarse dentro de `Services/`.

## WebApi

La capa `WebApi` es el punto de entrada de la aplicación y contiene las responsabilidades relacionadas con HTTP.

Estructura:

- `Controllers/` — Controladores HTTP responsables de recibir las solicitudes y devolver las respuestas.
- `Middlewares/` — Middlewares HTTP, como manejo global de excepciones, logging, autenticación y otras responsabilidades relacionadas con HTTP.

Los Controllers no deben contener lógica de negocio. Deben delegar las operaciones a los casos de uso correspondientes de la capa Application.

 
# Convenciones 


## Reglas
- No subir ni leer el .env
- No usar any a no ser estrictamente necesario

# Flujo de trabajo -
- Antes de una tarea no trivial, propón un plan y espera mi OK. 
- Una tarea a la vez; al terminar, dime qué cambiaste para que lo 
revise. 
- Si no estás seguro al 80%, pregunta. No inventes.

## Estructura de respuestas

- Errores (400, 401, 403, 404, 409, 500):
{
  status: 'error'
  message: ''
  code: 
}

- Exitosas (200, 201)
{
  status: 'success'
  message: ''
  code: 
  data: {}
}

  El 204 no devuelve nada





## ========================================================================================
