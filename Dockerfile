FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-api
WORKDIR /app

EXPOSE 80

COPY RequestService/*.csproj ./RequestService/

WORKDIR /app/RequestService
RUN dotnet restore

WORKDIR /app
COPY RequestService/ ./RequestService/

WORKDIR /app/RequestService
RUN dotnet publish /property:PublishWithAspNetCoreTargetManifest=false -c Release -o out


FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS final
WORKDIR /app

COPY --from=build-api /app/RequestService/out ./
ENTRYPOINT ["dotnet", "RequestService.dll"]
