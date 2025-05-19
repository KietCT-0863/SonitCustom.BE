# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["SonitCustom.sln", "./"]
COPY ["SonitCustom.Controller/SonitCustom.Controller.csproj", "SonitCustom.Controller/"]
COPY ["SonitCustom.BLL/SonitCustom.BLL.csproj", "SonitCustom.BLL/"]
COPY ["SonitCustom.DAL/SonitCustom.DAL.csproj", "SonitCustom.DAL/"]

# Restore NuGet packages
RUN dotnet restore

# Copy the rest of the source code
COPY . .

# Build and publish
RUN dotnet build "SonitCustom.sln" -c Release -o /app/build
RUN dotnet publish "SonitCustom.sln" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose port 80
EXPOSE 80

# Set environment variables
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

# Start the application
ENTRYPOINT ["dotnet", "SonitCustom.Controller.dll"] 