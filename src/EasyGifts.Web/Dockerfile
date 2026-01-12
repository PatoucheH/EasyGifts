# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files
COPY src/EasyGifts.Shared/EasyGifts.Shared.csproj src/EasyGifts.Shared/
COPY src/EasyGifts.UI/EasyGifts.UI.csproj src/EasyGifts.UI/
COPY src/EasyGifts.Web/EasyGifts.Web.csproj src/EasyGifts.Web/

# Restore dependencies
RUN dotnet restore src/EasyGifts.Web/EasyGifts.Web.csproj

# Copy source code
COPY src/ src/

# Build and publish
WORKDIR /src/src/EasyGifts.Web
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM nginx:alpine
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html

# Configure nginx for Blazor WASM
COPY src/EasyGifts.Web/nginx.conf /etc/nginx/nginx.conf

EXPOSE 80
