FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["Portfolio.API.csproj", "./"]
RUN dotnet restore "Portfolio.API.csproj"

COPY . .
RUN dotnet publish "Portfolio.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "Portfolio.API.dll"]
