# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy csproj(s) and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and publish
COPY . ./
RUN dotnet publish -c Release -o out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

# Use the PORT environment variable provided by Render
ENV ASPNETCORE_URLS=http://+:${PORT:-5000}

# Start the API
ENTRYPOINT ["dotnet", "Health-Api.dll"]
