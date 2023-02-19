# Geo.Monitoring.Blazor for Docker

## File docker images from local dev hub

### Save
docker save geomonitoringblazor:dev --output geomonitoringblazor.tar

### Load
docker load < geomonitoringblazor.tar


## Run from docker hub

docker run -d -p 45001:80 -e ASPNETCORE_ENVIRONMENT:Production -v /C/Progs/Geo/Geo.Monitoring.Blazor/Geo.Monitoring.Blazor/appsettings.Development.json:/app/appsettings.Production.json --name=Geo.Monitoring.Blazor.Dev3 sergeyvv1983/geomonitoringblazor:dev3