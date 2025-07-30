cd src
docker login
docker build -t jpjofresm/opa-logger-server:latest -t jpjofresm/opa-logger-server:1.01 -t opa-logger-server:latest -t opa-logger-server:1.01 .
docker push -a jpjofresm/opa-logger-server
cd ..