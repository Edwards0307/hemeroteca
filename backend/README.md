# Hemeroteca — Backend

API REST desarrollada con .NET 8 + Dapper + PostgreSQL.

## Requisitos
- .NET 8 SDK
- PostgreSQL 16

## Variables de entorno

Configura el archivo `appsettings.json` o variables de entorno:

| Variable | Descripción |
|---|---|
| `ConnectionStrings__DefaultConnection` | Cadena de conexión PostgreSQL |
| `Jwt__Key` | Clave secreta para JWT |

## Comandos

```bash
dotnet restore    # Restaurar dependencias
dotnet build      # Compilar
dotnet run        # Correr en desarrollo
```

## Estructura
Hemeroteca.API/
├── Controllers/      # Endpoints HTTP
├── Services/         # Lógica de negocio
│   └── Interfaces/
├── Repositories/     # Acceso a datos con Dapper
│   └── Interfaces/
├── Models/           # Modelos de datos
└── Common/           # Utilidades compartidas
## Endpoints principales

| Método | Ruta | Descripción |
|---|---|---|
| GET | /api/libros | Lista todos los libros |
| GET | /api/libros/{id} | Obtiene un libro |
| POST | /api/libros | Crea un libro |
| PUT | /api/libros/{id} | Actualiza un libro |
| DELETE | /api/libros/{id} | Elimina un libro |
| GET | /api/revistas | Lista todas las revistas |
| POST | /api/auth/login | Iniciar sesión |
| POST | /api/auth/registro | Registrar usuario |
