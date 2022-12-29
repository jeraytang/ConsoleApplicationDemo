FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["src/Pamirs.BI.Etl.App/Pamirs.BI.Etl.App.csproj", "Pamirs.BI.Etl.App/"]
RUN dotnet restore "src/Pamirs.BI.Etl.App/Pamirs.BI.Etl.App.csproj"
COPY . .
WORKDIR "/src/Pamirs.BI.Etl.App"
RUN dotnet build "Pamirs.BI.Etl.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pamirs.BI.Etl.App.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pamirs.BI.Etl.App.dll"]
