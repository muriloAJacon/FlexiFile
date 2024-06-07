#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Adapters/FlexiFile.Worker/FlexiFile.Worker.csproj", "Adapters/FlexiFile.Worker/"]
COPY ["Core/FlexiFile.Application/FlexiFile.Application.csproj", "Core/FlexiFile.Application/"]
COPY ["Core/FlexiFile.Core/FlexiFile.Core.csproj", "Core/FlexiFile.Core/"]
COPY ["Core/FlexiFile.Infrastructure/FlexiFile.Infrastructure.csproj", "Core/FlexiFile.Infrastructure/"]
RUN dotnet restore "./Adapters/FlexiFile.Worker/FlexiFile.Worker.csproj"
COPY . .
WORKDIR "/src/Adapters/FlexiFile.Worker"
RUN dotnet build "./FlexiFile.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FlexiFile.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FlexiFile.Worker.dll"]