# build-local-images.ps1
#
# This script builds the local Docker images for spidey-senses application
# Assumes your source code is in src/frontend and src/api

# Set error action preference to stop on any error
$ErrorActionPreference = "Stop"

# Variables
$frontendImageName = "spidey-senses/angular-js"
$frontendImageTag = "local"
$apiImageName = "spidey-senses/web-api-v2"
$apiImageTag = "local"
$frontendDir = "./src/frontend"
$apiDir = "./src/api"

Write-Host "=========================================================="
Write-Host "Building local Docker images for spidey-senses"
Write-Host "=========================================================="

function Exit-NoDocker {
    Write-Host "[ERROR] Docker is not running or not installed. Please start Docker." -ForegroundColor Red
    exit 1
}

# Check if Docker is installed and running
try {
    $dockerVersion = docker version --format '{{.Server.Version}}' 2>$null
    if ($dockerVersion) {
        Write-Host "[OK] Docker is running (version: $dockerVersion)" -ForegroundColor Green
    } else {
        Exit-NoDocker
    }
} catch {
    Exit-NoDocker
}

# Function to build an image
function Build-DockerImage {
    param (
        [string]$Directory,
        [string]$ImageName,
        [string]$Tag
    )
    
    $fullImageName = "$ImageName`:$Tag"
    
    if (Test-Path $Directory) {
        Write-Host "Building $fullImageName from $Directory..." -ForegroundColor Yellow
        try {
            docker build -t $fullImageName $Directory
            Write-Host "[OK] Successfully built $fullImageName" -ForegroundColor Green
            return $true
        } catch {
            Write-Host "[ERROR] Failed to build $fullImageName`: $_" -ForegroundColor Red
            return $false
        }
    } else {
        Write-Host "[ERROR] Directory $Directory not found." -ForegroundColor Red
        Write-Host "  Please make sure your source code is in the correct location." -ForegroundColor Yellow
        return $false
    }
}

# Build frontend image
$frontendBuilt = Build-DockerImage -Directory $frontendDir -ImageName $frontendImageName -Tag $frontendImageTag

# Build API image
$apiBuilt = Build-DockerImage -Directory $apiDir -ImageName $apiImageName -Tag $apiImageTag

# Summary
Write-Host "`n=========================================================="
if ($frontendBuilt -and $apiBuilt) {
    Write-Host "[OK] All images built successfully!" -ForegroundColor Green
    Write-Host "Run ./local-deploy.ps1 to deploy the application to Kubernetes"
} elseif ($frontendBuilt -or $apiBuilt) {
    Write-Host "[WARNING] Some images built successfully, but there were failures." -ForegroundColor Yellow
} else {
    Write-Host "[ERROR] Failed to build any images." -ForegroundColor Red
}
Write-Host "=========================================================="
