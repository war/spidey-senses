# Build local images for testing k8s

$ErrorActionPreference = "Stop"

$WebApiDockerfilePath = "./docker/api/web-api-v2/Dockerfile"
$AngularDockerfilePath = "./docker/frontend/angular-js/Dockerfile"

Write-Host "Building Web API Docker image..." -ForegroundColor Green
docker build -t spidey-senses/web-api-v2:local -f $WebApiDockerfilePath .
if (-not $?) {
    Write-Host "Failed to build Web API Docker image" -ForegroundColor Red
    exit 1
}

Write-Host "Building Angular Docker image..." -ForegroundColor Green
docker build -t spidey-senses/angular-js:local -f $AngularDockerfilePath .
if (-not $?) {
    Write-Host "Failed to build Angular Docker image" -ForegroundColor Red
    exit 1
}

Write-Host "Docker images built successfully:" -ForegroundColor Green
Write-Host "spidey-senses/web-api-v2:local" -ForegroundColor Cyan
Write-Host "spidey-senses/angular-js:local" -ForegroundColor Cyan
