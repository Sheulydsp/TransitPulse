# -------------------------
# Build stage
# -------------------------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY ["src/TransitPulse.API/TransitPulse.API.csproj", "src/TransitPulse.API/"]
COPY ["src/TransitPulse.Application/TransitPulse.Application.csproj", "src/TransitPulse.Application/"]
COPY ["src/TransitPulse.Domain/TransitPulse.Domain.csproj", "src/TransitPulse.Domain/"]
COPY ["src/TransitPulse.Infrastructure/TransitPulse.Infrastructure.csproj", "src/TransitPulse.Infrastructure/"]

RUN dotnet restore "src/TransitPulse.API/TransitPulse.API.csproj"

COPY . .

WORKDIR "/src/src/TransitPulse.API"

RUN dotnet build "TransitPulse.API.csproj" \
    --configuration Release \
    --no-restore \
    --output /app/build


# -------------------------
# Publish stage
# -------------------------
FROM build AS publish

RUN dotnet publish "TransitPulse.API.csproj" \
    --configuration Release \
    --no-restore \
    --output /app/publish \
    /p:UseAppHost=false


# -------------------------
# Runtime stage
# -------------------------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

COPY --from=publish /app/publish .

EXPOSE 8080

ENV ASPNETCORE_HTTP_PORTS=8080

ENTRYPOINT ["dotnet", "TransitPulse.API.dll"]