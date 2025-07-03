# API de Gestión de Usuarios

API Web desarrollada en .NET Core para la gestión de usuarios con operaciones CRUD básicas.

## Tecnologías Utilizadas

- .NET 7.0
- Entity Framework Core
- SQL Server
- ASP.NET Core Web API

## Patrones de Diseño Implementados

### **Patrón Singleton**

En el `ComparationService`, implementamos el patrón **Singleton** para asegurar que solo exista **una única instancia** del servicio durante la vida útil de la aplicación. Esto optimiza el uso de recursos y asegura que no haya inconsistencias en el estado del servicio al tener múltiples instancias.

1. **Instancia única**: Se creó una instancia estática del servicio que se accede a través de un método `Instance()`.
2. **Constructor privado**: El constructor de `ComparationService` es privado para evitar la creación de nuevas instancias desde fuera de la clase.
3. **Acceso global**: La instancia del servicio es accesible desde cualquier parte de la aplicación sin necesidad de recrearla.

### **Patrón Proxy**

El patrón **Proxy** se aplica al servicio `ICVService` mediante el `CVServiceProxy`. Este patrón permite añadir funcionalidades adicionales como **cacheo** y **logging** al servicio original, sin modificar el servicio real.

1. **Cacheo**: Los resultados de las operaciones `GetCV` se almacenan en memoria, mejorando la eficiencia al evitar consultas repetidas.
2. **Logging**: Cada operación en el servicio (agregar, obtener o eliminar CVs) es registrada en los logs, lo que facilita el monitoreo de la aplicación.
3. **Extensibilidad**: Se pueden agregar más decoradores (proxies) sin afectar el servicio original, permitiendo la inclusión de más responsabilidades como validación o control de acceso.


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
