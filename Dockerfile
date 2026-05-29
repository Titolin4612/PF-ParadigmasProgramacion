# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files
COPY ["MVC Web App/MVC_ProyectoFinalPOO.csproj", "MVC Web App/"]
COPY ["Class Library/CL_ProyectoFinalPOO.csproj", "Class Library/"]

# Restore dependencies
RUN dotnet restore "MVC Web App/MVC_ProyectoFinalPOO.csproj"

# Copy everything else
COPY . .

# Build and publish
WORKDIR "/src/MVC Web App"
RUN dotnet build "MVC_ProyectoFinalPOO.csproj" -c Release -o /app/build
RUN dotnet publish "MVC_ProyectoFinalPOO.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Create logs directory
RUN mkdir -p /app/logs

# Copy published app
COPY --from=build /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENV TZ=UTC

# Expose port
EXPOSE 8080

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

# Entry point
ENTRYPOINT ["dotnet", "MVC_ProyectoFinalPOO.dll"]
