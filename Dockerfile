FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Todo.Api/Todo.Api.csproj", "Todo.Api/"]
COPY ["src/Todo.Application/Todo.Application.csproj", "Todo.Application/"]
COPY ["src/Todo.Domain/Todo.Domain.csproj", "Todo.Domain/"]
COPY ["src/Todo.Contracts/Todo.Contracts.csproj", "Todo.Contracts/"]
COPY ["src/Todo.Infrastructure/Todo.Infrastructure.csproj", "Todo.Infrastructure/"]

RUN dotnet restore "Todo.Api/Todo.Api.csproj"

COPY . ../

WORKDIR /src/Todo.Api
RUN dotnet build "Todo.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish --no-restore -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0

ENV ASPNETCORE_HTTP_PORTS=5148
EXPOSE 5148

WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Todo.Api.dll"]
