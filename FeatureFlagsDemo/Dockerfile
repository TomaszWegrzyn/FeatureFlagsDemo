FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FeatureFlagsDemo/FeatureFlagsDemo.csproj", "FeatureFlagsDemo/"]
RUN dotnet restore "FeatureFlagsDemo/FeatureFlagsDemo.csproj"
COPY . .
WORKDIR "/src/FeatureFlagsDemo"
RUN dotnet build "FeatureFlagsDemo.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FeatureFlagsDemo.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FeatureFlagsDemo.dll"]
