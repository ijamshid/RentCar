# SDK image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files and restore
COPY ["src/RentCar.API/RentCar.API.csproj", "src/RentCar.API/"]
COPY ["src/RentCar.Application/RentCar.Application.csproj", "src/RentCar.Application/"]
COPY ["src/RentCar.DataAccess/RentCar.DataAccess.csproj", "src/RentCar.DataAccess/"]
COPY ["src/RentCar.Core/RentCar.Core.csproj", "src/RentCar.Core/"]
RUN dotnet restore "./src/RentCar.API/RentCar.API.csproj"

# Copy the rest and build
COPY . .
WORKDIR "/src/src/RentCar.API"
RUN dotnet publish "RentCar.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "RentCar.API.dll"]
