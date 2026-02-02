FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY src/Presentation/Portfolio.WebAPI/Portfolio.WebAPI.csproj ./Portfolio.WebAPI.csproj
RUN dotnet restore "Portfolio.WebAPI.csproj"

COPY src/Presentation/Portfolio.WebAPI/. .

RUN dotnet publish "Portfolio.WebAPI.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "Portfolio.WebAPI.dll"]
