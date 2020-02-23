# https://hub.docker.com/_/microsoft-dotnet-core
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY Retro/*.csproj ./Retro/
COPY Retro.Tests/*.csproj ./Retro.Tests/
RUN dotnet restore

# copy everything else and build app
COPY Retro/. ./Retro/
WORKDIR /source/Retro
RUN dotnet build
RUN dotnet publish -c release -o /app --no-restore

WORKDIR /source
COPY Retro.Tests/. ./Retro.Tests

FROM build AS testrunner
ENTRYPOINT ["dotnet", "Retro.Tests.dll"]
RUN dotnet test

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Retro.dll"]

