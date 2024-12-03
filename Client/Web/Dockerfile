FROM node:18.17.1 AS build
WORKDIR /app

COPY package*.json .

RUN npm install

COPY . . 
RUN npm run build

#nginx
FROM nginx:latest
COPY --from=build /app/dist/browser /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80