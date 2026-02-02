# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY ["SampleApp/BackEnd/BackEnd.csproj", "SampleApp/BackEnd/"]
RUN dotnet restore "SampleApp/BackEnd/BackEnd.csproj"

COPY . .
WORKDIR "/src/SampleApp/BackEnd"
RUN dotnet build "BackEnd.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "BackEnd.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0

WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 8081

ENV ASPNETCORE_URLS=http://+:8081
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "BackEnd.dll"]
