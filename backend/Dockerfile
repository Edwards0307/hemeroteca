FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY Hemeroteca.API/Hemeroteca.API.csproj Hemeroteca.API/
RUN dotnet restore Hemeroteca.API/Hemeroteca.API.csproj
COPY . .
RUN dotnet publish Hemeroteca.API/Hemeroteca.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Hemeroteca.API.dll"]
