docker rmi web:2.0
docker build -t web:2.0 -f ./Client/Web/Dockerfile ./Client/Web/
docker tag web:2.0 zg04ckpt/zlearn:web-2.0
docker push zg04ckpt/zlearn:web-2.0

