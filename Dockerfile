FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Oog.API.sln", "./"]
COPY ["Oog.API/Oog.API.csproj", "Oog.API/"]
COPY ["Oog.Domain/Oog.Domain.csproj", "Oog.Domain/"]
COPY ["Oog.Migrations/Oog.Migrations.csproj", "Oog.Migrations/"]

RUN dotnet restore

COPY . .
RUN dotnet build -c Release --no-restore
RUN dotnet publish "Oog.API/Oog.API.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

COPY aspnetapp.pfx /https/aspnetapp.pfx

ENV ASPNETCORE_URLS="http://localhost:80"
#ENV ASPNETCORE_HTTPS_PORT=443
#ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx
#ENV ASPNETCORE_Kestrel__Certificates__Default__Password=YourSecurePassword

EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "Oog.API.dll"]
