FROM node:23 AS build

# Update npm
RUN npm install -g npm@11.1.0

WORKDIR /app

# Copy the package jsons and install them
COPY src/frontend/angular-js/package*.json .
RUN npm install

# Copy all source files
COPY src/frontend/angular-js/ .

# Build the project
RUN npm run build

# Switch to nginx for hosting
FROM nginx:stable-alpine AS publish

WORKDIR /

# Copy built files over to nginx
COPY --from=build /app/dist/angular-js/browser/ /usr/share/nginx/html

EXPOSE 80

# Run nginx
CMD ["nginx", "-g", "daemon off;"]
