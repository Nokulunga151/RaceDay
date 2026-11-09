
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Copy project files
COPY ["RaceDay.Api/RaceDay.Api.csproj", "RaceDay.Api/"]
COPY ["RaceDay.Contracts/RaceDay.Contracts.csproj", "RaceDay.Contracts/"]

# Restore dependencies 
RUN dotnet restore "RaceDay.Api/RaceDay.Api.csproj"

# Copy the remaining source files
COPY . .

# Publish the API
WORKDIR /src/RaceDay.Api
RUN dotnet publish "RaceDay.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENV ASPNETCORE_HTTP_PORTS=8080

ENTRYPOINT ["dotnet", "RaceDay.Api.dll"]
