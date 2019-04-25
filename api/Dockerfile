FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["api/Talking.Api.csproj", "api/"]
RUN dotnet restore "api/Talking.Api.csproj"
COPY . .
WORKDIR "/src/api"
RUN dotnet build "Talking.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Talking.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Talking.Api.dll"]