#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["TestDrivingFun.WebUi/TestDrivingFun.WebUi.csproj", "TestDrivingFun.WebUi/"]
RUN dotnet restore "TestDrivingFun.WebUi/TestDrivingFun.WebUi.csproj"
COPY . .
WORKDIR "/src/TestDrivingFun.WebUi"
RUN dotnet build "TestDrivingFun.WebUi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestDrivingFun.WebUi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestDrivingFun.WebUi.dll"]