FROM node:23 AS build

# Update npm
RUN npm install -g npm@11.1.0

WORKDIR /app

# Copy the package jsons and install them
COPY src/frontend/angular-js/package*.json .
RUN npm install

# Copy all source files
COPY src/frontend/angular-js/ .

# Accept build arg with default value
ARG ANGULAR_ENV=local

# Build the project
RUN if [ "$ANGULAR_ENV" = "prod" ] ; then \
      npm run build:prod ; \
    elif [ "$ANGULAR_ENV" = "dev" ] ; then \
      npm run build:dev ; \
    elif [ "$ANGULAR_ENV" = "k8s-local" ] ; then \
      npm run build:k8s-local ; \
    else \
      npm run build:local ; \
    fi

# Switch to nginx for hosting
FROM nginx:stable-alpine AS publish

WORKDIR /

# Copy built files over to nginx
COPY --from=build /app/dist/angular-js/browser/ /usr/share/nginx/html

EXPOSE 80

# Run nginx
CMD ["nginx", "-g", "daemon off;"]
