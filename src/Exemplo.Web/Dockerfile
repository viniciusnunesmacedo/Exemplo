# Get Base Image (Full .NET Core SDK)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-exemplo
WORKDIR /app

# Copy csproj and restore
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Generate runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
EXPOSE 80
COPY --from=build-exemplo /app/out .
ENTRYPOINT [ "dotnet","Exemplo.dll" ]