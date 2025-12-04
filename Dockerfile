# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY QuoteManagement.sln ./
COPY src/QuoteManagement.API/QuoteManagement.API.csproj ./src/QuoteManagement.API/
COPY src/QuoteManagement.Application/QuoteManagement.Application.csproj ./src/QuoteManagement.Application/
COPY src/QuoteManagement.Domain/QuoteManagement.Domain.csproj ./src/QuoteManagement.Domain/
COPY src/QuoteManagement.Infrastructure/QuoteManagement.Infrastructure.csproj ./src/QuoteManagement.Infrastructure/

# Restore dependencies
RUN dotnet restore

# Copy everything else
COPY . .

# Build and publish
WORKDIR /src/src/QuoteManagement.API
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

# Render uses PORT environment variable
EXPOSE 10000
ENV ASPNETCORE_URLS=http://+:10000

ENTRYPOINT ["dotnet", "QuoteManagement.API.dll"]
