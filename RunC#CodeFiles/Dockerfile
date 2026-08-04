FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy project file and restore
COPY RunC#CodeFiles/RunC#CodeFiles.csproj ./RunC#CodeFiles/
RUN dotnet restore RunC#CodeFiles/RunC#CodeFiles.csproj

# Copy source files
COPY RunC#CodeFiles/ ./RunC#CodeFiles/

# Build the application
WORKDIR /app/RunC#CodeFiles
RUN dotnet build -c Release -o /app/output

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy built files
COPY --from=build /app/output/ .

# Ensure execute permissions if needed
RUN chmod +x /app/*.dll

EXPOSE 8080
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENTRYPOINT ["dotnet", "/app/RunC#CodeFiles.dll"]

