FROM node:slim as builder
RUN apt-get update -qq && apt-get install -y build-essential libpq-dev vim

WORKDIR /app

COPY web/package*.json /app/
RUN npm set progress=false && npm install
COPY web/ /app/
RUN npm run build

FROM nginx
RUN rm -rf /usr/share/nginx/html/*
COPY nginx.conf /etc/nginx/nginx.conf
COPY --from=builder /app/dist /usr/share/nginx/html
