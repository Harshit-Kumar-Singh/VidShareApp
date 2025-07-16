# --- Build Stage ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy .csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy rest of the code and publish
COPY . ./
RUN dotnet publish -c Release -o out

# --- Runtime Stage ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Expose port (from appsettings or default 80)
EXPOSE 80

ENTRYPOINT ["dotnet", "vidsharewebapi.dll"]
