FROM jpgouigoux/aspnet:1.0.0-beta4
ADD /src /app
WORKDIR /app
RUN ["dnu", "restore"]
EXPOSE 5004
ENTRYPOINT sleep 10000000 | dnx . kestrel
