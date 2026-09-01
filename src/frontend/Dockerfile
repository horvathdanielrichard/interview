# syntax=docker/dockerfile:1
FROM node:22-alpine AS build
WORKDIR /src

COPY package*.json ./
RUN npm ci

COPY . .
RUN npx ng build --configuration production

FROM nginx:1.27-alpine AS final
COPY --from=build /src/dist/interview-client/browser /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
