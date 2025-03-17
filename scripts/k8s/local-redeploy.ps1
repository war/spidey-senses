# Parse command line parameters
param (
    [switch]$SkipBuild = $false
)

# Set error action preference to stop on any error
$ErrorActionPreference = "Stop"

$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$buildScript = Join-Path -Path $scriptPath -ChildPath "local-build-images.ps1"
$cleanScript = Join-Path -Path $scriptPath -ChildPath "local-cleanup.ps1"
$deployScript = Join-Path -Path $scriptPath -ChildPath "local-deploy.ps1"

# Check if the required scripts exist
if (-not (Test-Path $buildScript)) {
    Write-Host "[ERROR] Build script not found at: $buildScript" -ForegroundColor Red
    exit 1
}

if (-not (Test-Path $cleanScript)) {
    Write-Host "[ERROR] Clean script not found at: $cleanScript" -ForegroundColor Red
    exit 1
}

if (-not (Test-Path $deployScript)) {
    Write-Host "[ERROR] Deploy script not found at: $deployScript" -ForegroundColor Red
    exit 1
}

Write-Host "=========================================================="
Write-Host "SPIDEY-SENSES FULL REDEPLOY PROCESS"
Write-Host "=========================================================="

# Function to execute a script and check its exit code
function Execute-Script {
    param (
        [string]$ScriptPath,
        [string]$Description,
        [bool]$Required = $true
    )
   
    if (-not (Test-Path $ScriptPath)) {
        if ($Required) {
            Write-Host "[ERROR] Required script not found: $ScriptPath" -ForegroundColor Red
            exit 1
        } else {
            Write-Host "[WARNING] Optional script not found: $ScriptPath" -ForegroundColor Yellow
            return $true
        }
    }
   
    Write-Host "`n=========================================================="
    Write-Host "STEP: $Description"
    Write-Host "==========================================================`n"
   
    & $ScriptPath
    $exitCode = $LASTEXITCODE
   
    if ($exitCode -ne 0) {
        Write-Host "[ERROR] Script failed with exit code: $exitCode" -ForegroundColor Red
        return $false
    }
   
    return $true
}

# Step 1: Build Docker images (if not skipped)
if (-not $SkipBuild) {
    if (-not (Execute-Script -ScriptPath $buildScript -Description "Building Docker images")) {
        Write-Host "[ERROR] Failed to build Docker images. Aborting redeploy." -ForegroundColor Red
        exit 1
    }
} else {
    Write-Host "`n=========================================================="
    Write-Host "STEP: Building Docker images (SKIPPED)"
    Write-Host "==========================================================`n"
}

# Step 2: Clean existing deployment
$cleanResult = Execute-Script -ScriptPath $cleanScript -Description "Cleaning existing deployment" -Required $false
if (-not $cleanResult) {
    $continue = Read-Host "Clean script failed or was cancelled. Continue with deployment anyway? (y/n)"
    if ($continue -ne "y") {
        Write-Host "Redeploy aborted by user." -ForegroundColor Yellow
        exit 0
    }
}

# Step 3: Deploy application
if (-not (Execute-Script -ScriptPath $deployScript -Description "Deploying full application")) {
    Write-Host "[ERROR] Failed to deploy application. Redeploy process failed." -ForegroundColor Red
    exit 1
}

Write-Host "`n=========================================================="
Write-Host "[SUCCESS] REDEPLOY PROCESS COMPLETED SUCCESSFULLY" -ForegroundColor Green
Write-Host "==========================================================`n"

# Show access URLs
Write-Host "Access your application at:"
Write-Host "  Frontend: http://spidey-senses.local/"
Write-Host "  API:      http://api.spidey-senses.local/"