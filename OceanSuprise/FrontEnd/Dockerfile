# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY ["SampleApp/FrontEnd/FrontEnd.csproj", "SampleApp/FrontEnd/"]
RUN dotnet restore "SampleApp/FrontEnd/FrontEnd.csproj"

COPY . .
WORKDIR "/src/SampleApp/FrontEnd"
RUN dotnet build "FrontEnd.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "FrontEnd.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0

WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENV WEATHER_URL=http://backend:8081

ENTRYPOINT ["dotnet", "FrontEnd.dll"]
