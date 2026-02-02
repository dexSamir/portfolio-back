FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY src/Presentation/Portfolio.WebApi/Portfolio.WebApi.csproj ./Portfolio.WebApi.csproj
RUN dotnet restore "Portfolio.WebApi.csproj"

COPY src/Presentation/Portfolio.WebApi/. .
RUN dotnet publish "Portfolio.WebApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "Portfolio.WebApi.dll"]
