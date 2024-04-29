FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

LABEL author="Jason Drawdy"
ENV ASPNETCORE_URLS=http://+:5000

WORKDIR /App
COPY ./AcmeCorpApi .
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/out .

ENV DOTNET_EnableDiagnostics=0
EXPOSE 5000
ENTRYPOINT ["dotnet", "AcmeCorpApi.dll"]