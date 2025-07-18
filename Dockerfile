# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

# Muhitni productionga sozlaymiz va faqat HTTP port ochamiz
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/RentCar.API/RentCar.API.csproj", "src/RentCar.API/"]
COPY ["src/RentCar.Application/RentCar.Application.csproj", "src/RentCar.Application/"]
COPY ["src/RentCar.DataAccess/RentCar.DataAccess.csproj", "src/RentCar.DataAccess/"]
COPY ["src/RentCar.Core/RentCar.Core.csproj", "src/RentCar.Core/"]
RUN dotnet restore "./src/RentCar.API/RentCar.API.csproj"
COPY . .
WORKDIR "/src/src/RentCar.API"
RUN dotnet build "./RentCar.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RentCar.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RentCar.API.dll"]
