FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Document-Analyser-Api/Document-Analyser-Api.csproj", "Document-Analyser-Api/"]
COPY ["Document-Analyser-Services/Document-Analyser-Services.csproj", "Document-Analyser-Services/"]
COPY ["Document-Analyser-Common/Document-Analyser-Common.csproj", "Document-Analyser-Common/"]
RUN dotnet restore "Document-Analyser-Api/Document-Analyser-Api.csproj"
COPY . .
WORKDIR "/src/Document-Analyser-Api"
RUN dotnet build "Document-Analyser-Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Document-Analyser-Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Document-Analyser-Api.dll"]