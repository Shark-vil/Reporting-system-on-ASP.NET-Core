FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN apt-get update \
    && apt-get install -y --no-install-recommends libgdiplus libc6-dev \
    && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["TESTING_WEB_API_ASP/TESTING_WEB_API_ASP.csproj", "TESTING_WEB_API_ASP/"]
RUN dotnet restore "TESTING_WEB_API_ASP/TESTING_WEB_API_ASP.csproj"
COPY . .
WORKDIR "/src/TESTING_WEB_API_ASP"
RUN dotnet build "TESTING_WEB_API_ASP.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "TESTING_WEB_API_ASP.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TESTING_WEB_API_ASP.dll"]