docker rmi api:2.0
docker build -t api:2.0 -f ./Server/Dockerfile.api ./Server/
docker tag api:2.0 zg04ckpt/zlearn:api-2.0
docker push zg04ckpt/zlearn:api-2.0