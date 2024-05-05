FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["balanceSimple/balanceSimple.csproj", "balanceSimple/"]
RUN dotnet restore "balanceSimple/balanceSimple.csproj"
COPY . .
WORKDIR "/src/balanceSimple"
RUN dotnet build "balanceSimple.csproj" -c Realese -o /app/build

FROM build as publish
RUN dotnet publish "balanceSimple.csproj" -c Realese -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "balanceSimple.dll"]
