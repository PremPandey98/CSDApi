FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

COPY ["CSDProject.API/CSDProject.API.csproj", "CSDProject.API/"]
COPY ["CSDProject.Application/CSDProject.Application.csproj", "CSDProject.Application/"]
COPY ["CSDProject.Domain/CSDProject.Domain.csproj", "CSDProject.Domain/"]
COPY ["CSDProject.Infrastructure/CSDProject.Infrastructure.csproj", "CSDProject.Infrastructure/"]

RUN dotnet restore "CSDProject.API/CSDProject.API.csproj"

COPY . .

WORKDIR "/src/CSDProject.API"

RUN dotnet publish "CSDProject.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0

WORKDIR /app

EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "CSDProject.API.dll"]