# FROM microsoft/dotnet:3.1-aspnetcore-runtime AS base
FROM mcr.microsoft.com/dotnet/aspnet:3.1-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# FROM microsoft/dotnet:2.1-sdk AS build
FROM mcr.microsoft.com/dotnet/sdk:3.1-alpine AS build
WORKDIR /src
COPY . .
RUN dotnet restore "api/Talking.Api.csproj"
WORKDIR "/src/api"
RUN dotnet build "Talking.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Talking.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Talking.Api.dll"]