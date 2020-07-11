FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build-env

WORKDIR /app
# Copy everything else and build
COPY . .

WORKDIR /app/Yape.Api

# Publish
RUN dotnet add package Microsoft.Packaging.Tools.Trimming --version 1.1.0-preview1-26619-01 --no-restore

RUN dotnet publish --runtime alpine-x64 -c Release \
	--self-contained true \
	-p:PublishTrimmed=true \
	-p:PublishSingleFile=true \
	-p:TrimUnusedDependencies=true \
	-o /app/out

# Build runtime image
FROM alpine:3.11

ENV TZ=America/Lima
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true

WORKDIR /app
EXPOSE 80

RUN apk update && apk add --no-cache libstdc++ libintl tzdata

COPY --from=build-env /app/out .

ENTRYPOINT ["./Yape.Api", "--urls", "http://0.0.0.0:80"]
