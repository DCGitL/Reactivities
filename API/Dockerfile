# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src



# Copy csproj and restore as distinct layers
COPY ["API/API.csproj", "API/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Persistence/Persistence.csproj", "Persistence/"]
COPY ["Domain/Domain.csproj", "Domain/"]

RUN dotnet restore "API/API.csproj"

# Copy the source code for these projects
COPY API/ API/
COPY Application/ Application/
COPY Infrastructure/ Infrastructure/
COPY Persistence/ Persistence/
COPY Domain/ Domain/

WORKDIR "/src/API"
RUN dotnet publish "API.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

COPY ./aspnetappApi.pfx /app/aspnetappApi.pfx

EXPOSE 5001
# Set environment variables for HTTPS
ENV ASPNETCORE_URLS=https://+:5001
ENV ASPNETCORE_HTTPS_PORT=5001

ENV ASPNETCORE_Kestrel__Certificates__Default__Password="testApi123"
ENV ASPNETCORE_Kestrel__Certificates__Default__Path="/app/aspnetappApi.pfx"

ENTRYPOINT ["dotnet", "API.dll"]