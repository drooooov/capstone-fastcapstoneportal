FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
# ENV ASPNETCORE_URLS http://*:5000
# ENV ASPNETCORE_ENVIRONMENT Production

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["FASTCapstonePortal/FASTCapstonePortal.csproj", "FASTCapstonePortal/"]
RUN dotnet restore "FASTCapstonePortal/FASTCapstonePortal.csproj"
COPY . .
WORKDIR "/src/FASTCapstonePortal"
RUN dotnet build "FASTCapstonePortal.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "FASTCapstonePortal.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
RUN mkdir -p /app/wwwroot/Images/
ENTRYPOINT ["dotnet", "FASTCapstonePortal.dll"]