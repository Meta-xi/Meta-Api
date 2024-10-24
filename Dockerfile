# Usar la imagen base de .NET 8 SDK para compilar el proyecto
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Establecer el directorio de trabajo
WORKDIR /app

# Copiar los archivos del proyecto y restaurar las dependencias
COPY *.csproj ./
RUN dotnet restore

# Copiar el resto del código y compilar
COPY . ./
RUN dotnet publish -c Release -o out

# Usar la imagen base de .NET 8 Runtime para la aplicación
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Establecer el directorio de trabajo
WORKDIR /app

# Copiar los archivos compilados desde la etapa de construcción
COPY --from=build /app/out .

# Exponer el puerto que usa la aplicación (cambiar si es necesario)
EXPOSE 5071

# Establecer el comando por defecto para ejecutar la aplicación
ENTRYPOINT ["dotnet", "Meta-xi.dll"]
