FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

ARG BUILD_CONFIGURATION=Release

# Copy centralized Packages directory 
COPY Directory.Packages.props ./Directory.Packages.props

COPY src/Domain/Domain.csproj src/Domain/
COPY src/Application/Application.csproj src/Application/
COPY src/Infrastructure/Infrastructure.csproj src/Infrastructure/
COPY src/SharedKernel/SharedKernel.csproj src/SharedKernel/
COPY src/Api/Api.csproj src/Api/

RUN dotnet restore src/Api/Api.csproj
COPY src/ src/

RUN dotnet publish src/Api/Api.csproj -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Create a non-root user and switch to it
RUN addgroup --system appgroup && adduser --system --ingroup appgroup appuser
USER appuser

COPY --from=build /app/publish .
ENV TZ="America/Santo_Domingo"
ENTRYPOINT ["dotnet", "Api.dll"]