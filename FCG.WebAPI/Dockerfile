# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiando todos os projetos necessários e o arquivo de solução
COPY FCG.WebAPI/ FCG.WebAPI/
COPY FCG.Application/ FCG.Application/
COPY FCG.Domain/ FCG.Domain/
COPY FCG.Infrastructure/ FCG.Infrastructure/

# Restaurar dependências e compilar
WORKDIR /src/FCG.WebAPI
RUN dotnet restore FCG.WebAPI.csproj
RUN dotnet publish FCG.WebAPI.csproj -c Release -o /app

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "FCG.WebAPI.dll"]