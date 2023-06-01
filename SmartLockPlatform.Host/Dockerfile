FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["SmartLockPlatform.Host/SmartLockPlatform.Host.csproj", "SmartLockPlatform.Host/"]
RUN dotnet restore "SmartLockPlatform.Host/SmartLockPlatform.Host.csproj"
COPY . .
WORKDIR "/src/SmartLockPlatform.Host"
RUN dotnet build "SmartLockPlatform.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmartLockPlatform.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartLockPlatform.Host.dll"]
