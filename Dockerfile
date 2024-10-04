# Use the SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Install EF Core tools in the build stage
RUN dotnet tool install --global dotnet-ef

# Copy the rest of the code
COPY . ./
RUN dotnet publish -c Release -o out

# Use the ASP.NET runtime image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out ./

# Set the entry point for the application
ENTRYPOINT ["dotnet", "MySimpleApi.dll"]
