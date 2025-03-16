FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

COPY *.sln ./
COPY spam_messenger/*.csproj spam_messenger/
RUN dotnet restore spam_messenger/spam_messenger.csproj

COPY . ./
WORKDIR /app/spam_messenger
RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

ENTRYPOINT ["dotnet", "spam_messenger.dll"]

