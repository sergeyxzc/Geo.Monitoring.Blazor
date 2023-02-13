# File docker images from local dev hub

## Save
docker save geomonitoringblazor:dev --output geomonitoringblazor.tar

## Load
docker load < geomonitoringblazor.tar
