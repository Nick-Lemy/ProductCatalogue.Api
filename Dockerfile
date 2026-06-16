# Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ProductCatalogue.Api/ProductCatalogue.Api.csproj ProductCatalogue.Api/
RUN dotnet restore ProductCatalogue.Api/ProductCatalogue.Api.csproj
COPY ProductCatalogue.Api/ ProductCatalogue.Api/
RUN dotnet publish ProductCatalogue.Api/ProductCatalogue.Api.csproj -c Release -o /app

# Run
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["sh", "-c", "exec dotnet ProductCatalogue.Api.dll --urls http://0.0.0.0:${PORT:-8080}"]
