# Set error action preference to stop on any error
$ErrorActionPreference = "Stop"

# Variables
$namespace = "spidey-senses"

function Show-Header {
    param(
        [string]$Text
    )
    
    Write-Host "`n=============================================================" -ForegroundColor Cyan
    Write-Host $Text -ForegroundColor Cyan
    Write-Host "=============================================================`n" -ForegroundColor Cyan
}

function Get-PodLogs {
    param(
        [string]$Label,
        [string]$Component,
        [switch]$Follow,
        [int]$TailLines = 100
    )
    
    $pods = kubectl get pods -n $namespace -l app=$Label -o name 2>$null
    
    if (-not $pods) {
        Write-Host "[WARNING] No $Component pods found in namespace $namespace" -ForegroundColor Yellow
        return $false
    }
    
    foreach ($pod in $pods) {
        $podName = $pod.Replace("pod/", "")
        Write-Host "[INFO] Showing logs for $Component pod: $podName" -ForegroundColor Green
        
        if ($Follow) {
            Write-Host "[INFO] Following logs... Press Ctrl+C to exit" -ForegroundColor Yellow
            kubectl logs -n $namespace $pod --tail=$TailLines -f
        } else {
            kubectl logs -n $namespace $pod --tail=$TailLines
        }
    }
    
    return $true
}

function Show-PodStatus {
    Show-Header "POD STATUS"
    
    kubectl get pods -n $namespace
    
    Write-Host "`n[INFO] Use 'kubectl describe pod -n $namespace <pod-name>' for more details" -ForegroundColor Gray
}

# Main menu
function Show-Menu {
    Show-Header "SPIDEY-SENSES LOG VIEWER"
    
    Write-Host "1. View API logs"
    Write-Host "2. View Frontend logs"
    Write-Host "3. View all logs"
    Write-Host "4. Follow API logs (live)"
    Write-Host "5. Follow Frontend logs (live)"
    Write-Host "6. Show pod status"
    Write-Host "7. Exit"
    
    $choice = Read-Host "`nEnter your choice (1-7)"
    
    switch ($choice) {
        "1" {
            Show-Header "API LOGS"
            Get-PodLogs -Label "api" -Component "API"
            Show-Menu
        }
        "2" {
            Show-Header "FRONTEND LOGS"
            Get-PodLogs -Label "frontend" -Component "Frontend"
            Show-Menu
        }
        "3" {
            Show-Header "ALL LOGS"
            $apiSuccess = Get-PodLogs -Label "api" -Component "API"
            $frontendSuccess = Get-PodLogs -Label "frontend" -Component "Frontend"
            
            if (-not ($apiSuccess -or $frontendSuccess)) {
                Write-Host "[WARNING] No pods found in namespace $namespace" -ForegroundColor Yellow
            }
            
            Show-Menu
        }
        "4" {
            Show-Header "LIVE API LOGS"
            Get-PodLogs -Label "api" -Component "API" -Follow
            Show-Menu
        }
        "5" {
            Show-Header "LIVE FRONTEND LOGS"
            Get-PodLogs -Label "frontend" -Component "Frontend" -Follow
            Show-Menu
        }
        "6" {
            Show-PodStatus
            Show-Menu
        }
        "7" {
            Write-Host "Exiting log viewer..." -ForegroundColor Gray
            return
        }
        default {
            Write-Host "[ERROR] Invalid choice. Please enter a number between 1 and 7." -ForegroundColor Red
            Show-Menu
        }
    }
}

# Check if kubectl is available
try {
    $null = kubectl version --client 2>$null
} catch {
    Write-Host "[ERROR] kubectl is not installed or not in your PATH. Please install kubectl first." -ForegroundColor Red
    exit 1
}

# Check if namespace exists
$namespaceExists = kubectl get namespace $namespace --ignore-not-found -o name 2>$null
if (-not $namespaceExists) {
    Write-Host "[ERROR] Namespace $namespace does not exist. Please deploy the application first." -ForegroundColor Red
    exit 1
}

# Start the menu
Show-Menu