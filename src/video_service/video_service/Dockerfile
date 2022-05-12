#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["video_service/video_service.csproj", "video_service/"]
RUN dotnet restore "video_service/video_service.csproj"
COPY . .
WORKDIR "/src/video_service"
RUN dotnet build "video_service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "video_service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "video_service.dll"]