#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["FPTShareLaptop_Controller/FPTShareLaptop_Controller.csproj", "FPTShareLaptop_Controller/"]
COPY ["Service/Service.csproj", "Service/"]
COPY ["BusinessObjects/BusinessObjects.csproj", "BusinessObjects/"]
COPY ["DataAccess/DataAccess.csproj", "DataAccess/"]
RUN dotnet restore "FPTShareLaptop_Controller/FPTShareLaptop_Controller.csproj"
COPY . .
WORKDIR "/src/FPTShareLaptop_Controller"
RUN dotnet build "FPTShareLaptop_Controller.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FPTShareLaptop_Controller.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FPTShareLaptop_Controller.dll"]