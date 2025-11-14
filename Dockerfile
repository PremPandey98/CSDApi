# Use the official .NET 9.0 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY CSDProject.sln .
COPY CSDProject.API/CSDProject.API.csproj CSDProject.API/
COPY CSDProject.Application/CSDProject.Application.csproj CSDProject.Application/
COPY CSDProject.Domain/CSDProject.Domain.csproj CSDProject.Domain/
COPY CSDProject.Infrastructure/CSDProject.Infrastructure.csproj CSDProject.Infrastructure/

# Restore dependencies
RUN dotnet restore

# Copy all source files
COPY . .

# Build and publish the application
WORKDIR /src/CSDProject.API
RUN dotnet publish -c Release -o /app/publish

# Use the official .NET 9.0 runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copy published application
COPY --from=build /app/publish .

# Create wwwroot/uploads directory for file storage
RUN mkdir -p wwwroot/uploads

# Expose port (Koyeb uses PORT environment variable)
EXPOSE 8000

# Set environment variable for port
ENV ASPNETCORE_URLS=http://+:8000

# Run the application
ENTRYPOINT ["dotnet", "CSDProject.API.dll"]
