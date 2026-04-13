# 📚 Hemeroteca Digital

Sistema de gestión de biblioteca digital con libros y revistas.

## 🛠️ Stack tecnológico

- **Frontend:** React 18 + TypeScript + Vite
- **Backend:** .NET 8 Web API
- **Base de datos:** PostgreSQL 16
- **ORM:** Dapper
- **Contenedores:** Docker + Docker Compose
- **CI/CD:** GitHub Actions

## 🚀 Correr el proyecto localmente

### Requisitos
- Docker Desktop
- .NET 8 SDK
- Node.js 20+

### Pasos

```bash
# 1. Clonar el repositorio
git clone git@github.com:Edwards0307/hemeroteca.git
cd hemeroteca

# 2. Levantar la base de datos
docker compose up postgres -d

# 3. Correr el backend
cd backend/Hemeroteca.API
dotnet run

# 4. Correr el frontend
cd frontend
npm install
npm run dev
```

## 📁 Estructura del proyecto
hemeroteca/
├── backend/               # .NET Web API
│   └── Hemeroteca.API/
│       ├── Controllers/   # Endpoints HTTP
│       ├── Services/      # Lógica de negocio
│       ├── Repositories/  # Acceso a datos con Dapper
│       └── Models/        # Modelos de datos
├── frontend/              # React + TypeScript
│   └── src/
│       ├── features/      # Módulos por funcionalidad
│       ├── components/    # Componentes reutilizables
│       ├── services/      # Llamadas al API
│       └── models/        # Tipos TypeScript
└── docker-compose.yml     # Orquestación de contenedores

## 🌿 Flujo de trabajo

- `main` → producción
- `develop` → integración
- `feature/xxx` → nuevas funcionalidades
