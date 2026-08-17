
# Build stage

FROM mcr.microsoft.com/dotnet/sdk:8.0.414 AS build

WORKDIR /src

# Copy project files first for better Docker layer caching
COPY ["CRN.API/CRN.API.csproj", "CRN.API/"]
COPY ["CRN.Application/CRN.Application.csproj", "CRN.Application/"]
COPY ["CRN.Domain/CRN.Domain.csproj", "CRN.Domain/"]
COPY ["CRN.Infrastructure/CRN.Infrastructure.csproj", "CRN.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "CRN.API/CRN.API.csproj"

# Copy source code
COPY . .

# Build and publish
WORKDIR "/src/CRN.API"
RUN dotnet publish "CRN.API.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore



# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0.20 AS final

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "CRN.API.dll"]