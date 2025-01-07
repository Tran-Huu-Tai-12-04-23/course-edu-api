# Sử dụng image .NET SDK 8.0 để build ứng dụng
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Sao chép file project và restore các package
COPY *.csproj ./
RUN dotnet restore

# Sao chép toàn bộ source code và build ứng dụng
COPY . ./
RUN dotnet publish -c Release -o out

# Sử dụng image runtime nhẹ để chạy ứng dụng
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Sao chép file đã build từ bước trước vào container
COPY --from=build /app/out .

# Expose cổng để ứng dụng lắng nghe
EXPOSE 80

# Command để chạy ứng dụng
ENTRYPOINT ["dotnet", "YourAppName.dll"]
