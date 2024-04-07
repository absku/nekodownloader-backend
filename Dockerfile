FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy AS base
WORKDIR /app
RUN mkdir -p /mnt/mangas
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 80


FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS build
WORKDIR /src
COPY ["NekoDownloader.Core/NekoDownloader.Core.csproj", "NekoDownloader.Core/"]
COPY ["NekoDownloader.Infrastructure/NekoDownloader.Infrastructure.csproj", "NekoDownloader.Infrastructure/"]
COPY ["NekoDownloader.Services/NekoDownloader.Services.csproj", "NekoDownloader.Services/"]
COPY ["NekoDownloader.Web/NekoDownloader.Web.csproj", "NekoDownloader.Web/"]
RUN dotnet restore "NekoDownloader.Web/NekoDownloader.Web.csproj"
COPY . .
WORKDIR "/src/NekoDownloader.Web"
RUN dotnet build "NekoDownloader.Web.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "NekoDownloader.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NekoDownloader.Web.dll"]
