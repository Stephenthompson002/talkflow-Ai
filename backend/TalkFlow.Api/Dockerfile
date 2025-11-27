# ---------- Stage 1: Build ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY ["TalkFlow.Api.csproj", "."]
RUN dotnet restore "TalkFlow.Api.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src"
RUN dotnet build "TalkFlow.Api.csproj" -c Release -o /app/build

# ---------- Stage 2: Publish ----------
FROM build AS publish
RUN dotnet publish "TalkFlow.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ---------- Stage 3: Runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Create non-root user
RUN groupadd -r appuser && useradd -r -g appuser appuser \
    && chown -R appuser:appuser /app

# Install curl for health checks
RUN apt-get update && apt-get install -y curl --no-install-recommends && rm -rf /var/lib/apt/lists/*

# Copy from publish stage
COPY --from=publish --chown=appuser:appuser /app/publish .

USER appuser

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

EXPOSE 8080
ENV ASPNETCORE_URLS=http://*:8080

ENTRYPOINT ["dotnet", "TalkFlow.Api.dll"]