# .\scripts\docker\docker-build-inspect.ps1 -ImageName "web-api-v2" -DockerfilePath "docker/api/web-api-v2/Dockerfile" -NoCache

param(
    [Parameter(Mandatory=$true)]
    [string]$ImageName,
    
    [Parameter(Mandatory=$false)]
    [string]$BuildStage = "",
    
    [Parameter(Mandatory=$true)]
    [string]$DockerfilePath,
    
    [Parameter(Mandatory=$false)]
    [switch]$NoCache
)

# Validate parameters
if (-not (Test-Path $DockerfilePath)) {
    Write-Error "Dockerfile not found at path: $DockerfilePath"
    exit 1
}

# Construct the build command
$buildCommand = "docker build"

# Add the target stage if specified
if ($BuildStage -and $BuildStage -ne "") {
    Write-Host "Building specific stage: $BuildStage" -ForegroundColor Yellow
    $buildCommand += " --target $BuildStage"
} else {
    Write-Host "Building entire Dockerfile (all stages)" -ForegroundColor Yellow
}

# Add no-cache option if specified
if ($NoCache) {
    Write-Host "Building without cache" -ForegroundColor Yellow
    $buildCommand += " --no-cache"
}

# Add context and dockerfile path
$buildCommand += " -t $ImageName -f $DockerfilePath ."

# Display the command being executed
Write-Host "Executing: $buildCommand" -ForegroundColor Cyan

# Execute the build command
Invoke-Expression $buildCommand

Write-Host "Build successful. Starting container in interactive mode..." -ForegroundColor Green

# Run the container in interactive mode with a shell
$shellCommand = "docker run --rm -it $ImageName /bin/bash"

# Try with bash first, if that fails, try with sh
try {
    Invoke-Expression $shellCommand

    if ($LASTEXITCODE -ne 0) {
        Write-Host "Bash not available, trying with /bin/sh..." -ForegroundColor Yellow
        Invoke-Expression "docker run --rm -it $ImageName /bin/sh"
    }
} catch {
    Write-Host "Could not start bash, trying with /bin/sh..." -ForegroundColor Yellow
    Invoke-Expression "docker run --rm -it $ImageName /bin/sh"
}
