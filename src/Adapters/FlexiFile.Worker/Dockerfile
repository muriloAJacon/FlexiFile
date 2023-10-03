#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Adapters/FlexiFile.Worker/FlexiFile.Worker.csproj", "Adapters/FlexiFile.Worker/"]
RUN dotnet restore "Adapters/FlexiFile.Worker/FlexiFile.Worker.csproj"
COPY . .
WORKDIR "/src/Adapters/FlexiFile.Worker"
RUN dotnet build "FlexiFile.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FlexiFile.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FlexiFile.Worker.dll"]