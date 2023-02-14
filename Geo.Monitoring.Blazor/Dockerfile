#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["../../devexpress-nuget-pkgs", "./devexpress-nuget-pkgs"]
COPY ["../../nuget.config", "./nuget.config"]

COPY ["Geo.Monitoring.Blazor/Geo.Monitoring.Blazor.csproj", "Geo.Monitoring.Blazor/"]
RUN dotnet restore "Geo.Monitoring.Blazor/Geo.Monitoring.Blazor.csproj"
COPY . .
WORKDIR "/src/Geo.Monitoring.Blazor"
RUN dotnet build "Geo.Monitoring.Blazor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Geo.Monitoring.Blazor.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Geo.Monitoring.Blazor.dll"]