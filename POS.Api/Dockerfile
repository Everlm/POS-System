
# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files first to leverage Docker layer caching
COPY ["POS.Api/POS.API.csproj", "POS.Api/"]
COPY ["POS.Application/POS.Application.csproj", "POS.Application/"]
COPY ["POS.Domain/POS.Domain.csproj", "POS.Domain/"]
COPY ["POS.Infrastructure/POS.Infrastructure.csproj", "POS.Infrastructure/"]
COPY ["POS.Utilities/POS.Utilities.csproj", "POS.Utilities/"]
RUN dotnet restore "./POS.Api/POS.API.csproj"

# Copy the rest of the source code
COPY . .
WORKDIR "/src/POS.Api"

# Publish the application, which also builds it
RUN dotnet publish "./POS.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 2: Create the final production image  with HTTPS certificate
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app

# Install openssl and generate self-signed cert
RUN apt-get update && apt-get install -y openssl \
    && mkdir -p /https \
    && openssl req -x509 -nodes -days 365 \
       -subj "/CN=localhost" \
       -newkey rsa:2048 -keyout /https/aspnetapp.key \
       -out /https/aspnetapp.crt \
    && openssl pkcs12 -export -out /https/aspnetapp.pfx \
       -inkey /https/aspnetapp.key -in /https/aspnetapp.crt \
       -passout pass:YourPassword \
    && chown -R 5678:5678 /https \
    && apt-get purge -y --auto-remove openssl \
    && rm -rf /var/lib/apt/lists/*  

# Create a non-root user for security
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

# Copy the published output from the build stage
COPY --from=build /app/publish .

# Expose the port the app runs on
# EXPOSE 80
# EXPOSE 443

ENTRYPOINT ["dotnet", "POS.API.dll"]