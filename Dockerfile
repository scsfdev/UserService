# UserService/Dockerfile

# ---------- build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# ---------- 1. Build Shared.Events ----------
COPY Shared.Events/Shared.Events.sln Shared.Events/
COPY Shared.Events/Shared.Events/Shared.Events.csproj Shared.Events/Shared.Events/
COPY Shared.Events/Shared.Events/ Shared.Events/Shared.Events/

# Build Shared.Events
RUN dotnet publish Shared.Events/Shared.Events/Shared.Events.csproj -c Release -o Shared.Events/Shared.Events/bin/Release/net9.0 --no-self-contained


# ---------- 2. Build UserService ----------
# Copy solution and project files for restore caching
COPY UserService/UserService.sln UserService/
COPY UserService/UserService.Api/UserService.Api.csproj UserService/UserService.Api/
COPY UserService/UserService.Application/UserService.Application.csproj UserService/UserService.Application/
COPY UserService/UserService.Domain/UserService.Domain.csproj UserService/UserService.Domain/
COPY UserService/UserService.Infrastructure/UserService.Infrastructure.csproj UserService/UserService.Infrastructure/

# Restoring API will auto restore related dependencies.
RUN dotnet restore UserService/UserService.Api/UserService.Api.csproj

# Copy rest of source
COPY UserService/UserService.Api/ UserService/UserService.Api/
COPY UserService/UserService.Application/ UserService/UserService.Application/
COPY UserService/UserService.Domain/ UserService/UserService.Domain/
COPY UserService/UserService.Infrastructure/ UserService/UserService.Infrastructure/

# Publish
RUN dotnet publish UserService/UserService.Api/UserService.Api.csproj -c Release -o /app --no-restore


# ---------- runtime stage ----------
# Final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

# 1. Add a non-root user named 'appuser'
RUN adduser --disabled-password --gecos "" --no-create-home appuser

WORKDIR /app

# 2. Change ownership of the /app directory to the new user.
RUN chown -R appuser:appuser /app

# 3. Copy published output
COPY --from=build /app ./

# 4. Expose the port.
EXPOSE 8080

# 5. Switch to the non-root user
USER appuser

# 6. Set the final entrypoint
ENTRYPOINT ["dotnet", "UserService.Api.dll"]