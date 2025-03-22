#!/bin/sh
set -e

# Default to local if ANGULAR_ENV is not set
ENV_NAME=${ANGULAR_ENV:-local}

echo "Setting Angular environment to: $ENV_NAME"

# Check if the requested environment file exists
if [ ! -f /usr/share/nginx/html/assets/environments/environment.${ENV_NAME}.ts ]; then
  echo "Warning: environment.${ENV_NAME}.ts not found, falling back to environment.local.ts"
  ENV_NAME="local"
fi

# Create/update the active environment link
cp /usr/share/nginx/html/assets/environments/environment.${ENV_NAME}.ts /usr/share/nginx/html/assets/environments/environment.ts

# Execute the main container command
exec "$@"
