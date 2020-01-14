#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Nhea.CoreCommunicationService/Nhea.CoreCommunicationService.csproj", "Nhea.CoreCommunicationService/"]
COPY ["Nhea/Nhea.csproj", "Nhea/"]
RUN dotnet restore "Nhea.CoreCommunicationService/Nhea.CoreCommunicationService.csproj"
COPY . .
WORKDIR "/src/Nhea.CoreCommunicationService"
RUN dotnet build "Nhea.CoreCommunicationService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Nhea.CoreCommunicationService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Nhea.CoreCommunicationService.dll"]