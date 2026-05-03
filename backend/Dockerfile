# Estapa 1 - compilar
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# Usa el SDk completo para compilar

WORKDIR /src
COPY Hemeroteca.API/Hemeroteca.API.csproj Hemeroteca.API/
RUN dotnet restore Hemeroteca.API/Hemeroteca.API.csproj
# Restaura los paquetes Nuget primero (Optimizacion de cache)

COPY . .
RUN dotnet publish Hemeroteca.API/Hemeroteca.API.csproj -c Release -o /app/publish
# Compilay publica en modo release

#Etapa 2 - correr
FROM mcr.microsoft.com/dotnet/aspnet:8.0
# Solo el runtine, mas liviado que el SDK completo

WORKDIR /app
COPY --from=build /app/publish .
# Copia solo el resultado copiado

ENTRYPOINT ["dotnet", "Hemeroteca.API.dll"]
# Comando que se ejecuta al iniciar el contenedor
