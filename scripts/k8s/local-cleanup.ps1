$ErrorActionPreference = "Stop"

$namespace = "spidey-senses-local"
$kubeContext = kubectl config current-context

Write-Host "=========================================================="
Write-Host "Cleaning up spidey-senses deployment"
Write-Host "Using Kubernetes context: $kubeContext"
Write-Host "=========================================================="

function Test-NamespaceExists {
    param (
        [string]$Namespace
    )
    
    $output = kubectl get namespace $Namespace --ignore-not-found 2>$null
    return $output -ne $null -and $output -ne ""
}

if (Test-NamespaceExists -Namespace $namespace) {
    $confirm = Read-Host "Are you sure you want to delete the entire $namespace namespace? (y/n)"
    
    if ($confirm -eq "y") {
        Write-Host "Deleting namespace $namespace..." -ForegroundColor Yellow
        kubectl delete namespace $namespace
        
        $retries = 0
        $maxRetries = 30
        while ((Test-NamespaceExists -Namespace $namespace) -and ($retries -lt $maxRetries)) {
            Write-Host "Waiting for namespace $namespace to be fully deleted... ($retries/$maxRetries)" -ForegroundColor Yellow
            Start-Sleep -Seconds 2
            $retries++
        }
        
        if (Test-NamespaceExists -Namespace $namespace) {
            Write-Host "[ERROR] Timed out waiting for namespace $namespace to be deleted." -ForegroundColor Red
            exit 1
        } else {
            Write-Host "[OK] Namespace $namespace and all resources have been deleted" -ForegroundColor Green
        }
    } else {
        Write-Host "Operation cancelled." -ForegroundColor Yellow
    }
} else {
    Write-Host "[OK] Namespace $namespace does not exist. Nothing to clean up." -ForegroundColor Green
}

Write-Host "=========================================================="
