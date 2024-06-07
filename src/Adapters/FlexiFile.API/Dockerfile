#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Adapters/FlexiFile.API/FlexiFile.API.csproj", "Adapters/FlexiFile.API/"]
COPY ["Core/FlexiFile.Application/FlexiFile.Application.csproj", "Core/FlexiFile.Application/"]
COPY ["Core/FlexiFile.Core/FlexiFile.Core.csproj", "Core/FlexiFile.Core/"]
COPY ["Core/FlexiFile.Infrastructure/FlexiFile.Infrastructure.csproj", "Core/FlexiFile.Infrastructure/"]
RUN dotnet restore "./Adapters/FlexiFile.API/FlexiFile.API.csproj"
COPY . .
WORKDIR "/src/Adapters/FlexiFile.API"
RUN dotnet build "./FlexiFile.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FlexiFile.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FlexiFile.API.dll"]