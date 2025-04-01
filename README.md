# API de Gestión de Usuarios

API Web desarrollada en .NET Core para la gestión de usuarios con operaciones CRUD básicas.

## Tecnologías Utilizadas

- .NET 7.0
- Entity Framework Core
- SQL Server
- ASP.NET Core Web API

## Endpoints de la API

### Usuarios

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET    | `/api/Users` | Obtener todos los usuarios |
| GET    | `/api/Users/{id}` | Obtener usuario por ID |
| POST   | `/api/Users` | Crear nuevo usuario |
| PUT    | `/api/Users/{id}` | Actualizar usuario existente |
| DELETE | `/api/Users/{id}` | Eliminar usuario |

## Modelo de Datos

csharp
public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string NumeroTelefono { get; set; }
    public DateTime Cumpleaños { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

## Campos Requeridos
- FirstName (Nombre)
- LastName (Apellido)
- Email (Correo electrónico)
- NumeroTelefono
- Cumpleaños

## Instrucciones de Configuración
1. Clonar el repositorio
2. Actualizar la cadena de conexión en appsettings.json
3. Ejecutar las migraciones de Entity Framework:

## Formatos de Respuesta
### Respuesta Exitosa

{
    "id": 1,
    "firstName": "Juan",
    "lastName": "Pérez",
    "email": "juan@ejemplo.com",
    "numeroTelefono": "1234567890",
    "cumpleaños": "1990-01-01T00:00:00",
    "isActive": true,
    "createdAt": "2024-01-01T00:00:00"
}
