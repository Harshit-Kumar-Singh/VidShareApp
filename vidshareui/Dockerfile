FROM node:20-alpine AS builder
WORKDIR /app
COPY . .
RUN npm i
RUN npm run build --output-path=dist


FROM nginx:alpine
COPY --from=builder /app/dist/vidshareui /usr/share/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf