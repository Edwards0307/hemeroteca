# Hemeroteca — Frontend

Interfaz web desarrollada con React 18 + TypeScript + Vite.

## Requisitos
- Node.js 20+

## Instalación

```bash
npm install
```

## Variables de entorno

Copia el archivo de ejemplo y configura tus variables:

```bash
cp .env.example .env
```

| Variable | Descripción |
|---|---|
| `VITE_API_URL` | URL del backend API |
| `VITE_APP_NAME` | Nombre de la aplicación |

## Comandos

```bash
npm run dev      # Desarrollo local
npm run build    # Build para producción
npm run preview  # Preview del build
```

## Estructura
src/
├── features/      # Módulos por funcionalidad
├── components/    # Componentes reutilizables
├── layouts/       # Layout principal
├── views/         # Vistas globales
├── services/      # Llamadas al API
└── models/        # Tipos TypeScript
