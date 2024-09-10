FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY src/*.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY /src/. ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine

WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "NotificationService.dll"]