FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY ./EasyGiftsBackend.Domain/*.csproj ./EasyGiftsBackend.Domain/
COPY ./EasyGiftsBackend.Application/*.csproj ./EasyGiftsBackend.Application/
COPY ./EasyGiftsBackend.Infrastructure/*.csproj ./EasyGiftsBackend.Infrastructure/
COPY ./EasyGiftsBackend.Api/*.csproj ./EasyGiftsBackend.Api/

RUN dotnet restore ./EasyGiftsBackend.Api/EasyGiftsBackend.Api.csproj

COPY . .

RUN dotnet publish ./EasyGiftsBackend.Api/EasyGiftsBackend.Api.csproj -c Release -o /app/publish
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5000
COPY --from=build /app/publish .

EXPOSE 5000
ENTRYPOINT ["dotnet", "EasyGiftsBackend.Api.dll"]
