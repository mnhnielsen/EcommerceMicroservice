#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["sdu.bachelor.microservice.catalog/sdu.bachelor.microservice.catalog.csproj", "sdu.bachelor.microservice.catalog/"]
COPY ["sdu.bachelor.microservice.common/sdu.bachelor.microservice.common.csproj", "sdu.bachelor.microservice.common/"]
RUN dotnet restore "sdu.bachelor.microservice.catalog/sdu.bachelor.microservice.catalog.csproj"
COPY . .
WORKDIR "/src/sdu.bachelor.microservice.catalog"
RUN dotnet build "sdu.bachelor.microservice.catalog.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "sdu.bachelor.microservice.catalog.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "sdu.bachelor.microservice.catalog.dll"]