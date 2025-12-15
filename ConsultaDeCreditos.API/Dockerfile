FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["ConsultaDeCreditos.API/ConsultaDeCreditos.API.csproj", "ConsultaDeCreditos.API/"]
COPY ["ConsultaDeCreditos.Application/ConsultaDeCreditos.Application.csproj", "ConsultaDeCreditos.Application/"]
COPY ["ConsultaDeCreditos.Domain/ConsultaDeCreditos.Domain.csproj", "ConsultaDeCreditos.Domain/"]
COPY ["ConsultaDeCreditos.Infrastructure/ConsultaDeCreditos.Infrastructure.csproj", "ConsultaDeCreditos.Infrastructure/"]
COPY ["ConsultaDeCreditos.IoC/ConsultaDeCreditos.IoC.csproj", "ConsultaDeCreditos.IoC/"]

RUN dotnet restore "ConsultaDeCreditos.API/ConsultaDeCreditos.API.csproj"

COPY . .

WORKDIR "/src/ConsultaDeCreditos.API"
RUN dotnet build "ConsultaDeCreditos.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ConsultaDeCreditos.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN mkdir -p /app/data

ENTRYPOINT ["dotnet", "ConsultaDeCreditos.API.dll"]