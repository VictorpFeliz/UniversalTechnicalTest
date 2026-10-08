# Universal Technical Test

API RESTful desarrollada en .NET 8 como parte de una evaluación técnica.

## Tecnologías utilizadas

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core InMemory
- BCrypt.Net-Next
- JWT Authentication
- FluentValidation
- Swagger / OpenAPI
- xUnit
- HttpClientFactory

## Estructura del proyecto

```text
UniversalTechnicalTest
├── UniversalTechnicalTest.Api
└── UniversalTechnicalTest.Tests
```

## Requisitos

- .NET 8 SDK
- Visual Studio 2022 o compatible

## Ejecución

1. Clonar el repositorio.
2. Abrir la solución `UniversalTechnicalTest.sln`.
3. Restaurar los paquetes NuGet.
4. Ejecutar el proyecto `UniversalTechnicalTest.Api`.

La aplicación utiliza Entity Framework Core InMemory, por lo que los datos almacenados se eliminan al detener la aplicación.

## Configuración

La configuración principal se encuentra en `appsettings.json`.

Incluye:

- Nombre de la base de datos InMemory.
- Expresiones regulares utilizadas para validación.
- Configuración de JWT.
- Tiempo de expiración del token.

## Endpoints

### Registro de usuario

```http
POST /api/Auth/register
```

Ejemplo:

```json
{
  "name": "Ramon Perez",
  "email": "ramonperez@gmail.com",
  "password": "Password@123"
}
```

El endpoint valida:

- Nombre obligatorio.
- Formato válido de correo electrónico.
- Contraseña con más de 8 caracteres.
- Uso de mayúsculas.
- Uso de minúsculas.
- Uso de símbolos.
- Correo electrónico no registrado previamente.

La contraseña es almacenada utilizando BCrypt.

La respuesta incluye:

- Identificador único del usuario.
- Nombre.
- Correo electrónico.
- Token JWT.

### Inicio de sesión

```http
POST /api/Auth/login
```

Ejemplo:

```json
{
  "email": "ramonperez@gmail.com",
  "password": "Password@123"
}
```

Si las credenciales son válidas, se retorna un nuevo token JWT.

## Autenticación

Los endpoints relacionados con posts requieren autenticación mediante JWT.

El token debe enviarse mediante el header:

```http
Authorization: Bearer <token>
```

Swagger también permite utilizar el botón `Authorize` para establecer el token JWT y probar los endpoints protegidos.

## Posts

### Obtener posts

```http
GET /api/Posts
```

Consume la API externa:

```text
https://jsonplaceholder.typicode.com/posts
```

Este endpoint requiere autenticación JWT.

### Crear post

```http
POST /api/Posts
```

Ejemplo:

```json
{
  "title": "Technical Test",
  "body": "Post created from the Universal technical assessment",
  "userId": 1
}
```

La información es enviada a JSONPlaceholder y la respuesta del servicio externo es retornada al cliente.

> JSONPlaceholder simula la creación de recursos y no persiste realmente los nuevos posts.

## Manejo de errores

La aplicación utiliza un middleware global para manejar excepciones de forma centralizada.

Ejemplos:

- `400 Bad Request` para datos inválidos o correos duplicados.
- `401 Unauthorized` cuando se intenta acceder a endpoints protegidos sin un JWT válido.
- `500 Internal Server Error` para errores inesperados.

## Pruebas unitarias

El proyecto `UniversalTechnicalTest.Tests` utiliza xUnit.

Las pruebas incluidas cubren:

- Registro válido.
- Almacenamiento de contraseña mediante hash.
- Registro con correo duplicado.
- Login válido.
- Login con contraseña incorrecta.

Para ejecutar las pruebas desde la terminal:

```bash
dotnet test
```

También pueden ejecutarse utilizando `Test Explorer` en Visual Studio.

## Versionamiento

El proyecto fue desarrollado utilizando GitHub Flow.

Las funcionalidades fueron trabajadas mediante ramas independientes y posteriormente integradas a la rama principal mediante Pull Requests.