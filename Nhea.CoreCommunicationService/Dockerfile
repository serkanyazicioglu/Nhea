FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build

WORKDIR /src
COPY . .

ARG OUTPROJECTFILE="./Nhea.CoreCommunicationService/Nhea.CoreCommunicationService.csproj"

RUN dotnet restore ${OUTPROJECTFILE}
RUN dotnet build ${OUTPROJECTFILE} -c Release -o /app --no-restore
RUN dotnet publish  ${OUTPROJECTFILE} -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Nhea.CoreCommunicationService.dll"]