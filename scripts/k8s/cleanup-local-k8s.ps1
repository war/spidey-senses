# Clean up local Kubernetes resources

$ErrorActionPreference = "Stop"

# Check if kubectl is installed
try {
    kubectl version --client | Out-Null
}
catch {
    Write-Host "kubectl not found. Please install kubectl and configure it for your Docker Desktop Kubernetes cluster." -ForegroundColor Red
    exit 1
}

# Delete all resources in the namespace
Write-Host "Deleting all resources in spidey-senses-local namespace..." -ForegroundColor Yellow

# Check if namespace exists first
$namespaceExists = $null
try {
    $namespaceExists = kubectl get namespace spidey-senses-local -o name 2>$null
} catch {
    # Ignore errors
}

if (-not $namespaceExists) {
    Write-Host "Namespace spidey-senses-local does not exist. Nothing to clean up." -ForegroundColor Green
    exit 0
}

# Delete the namespace if it exists
kubectl delete namespace spidey-senses-local

if (-not $?) {
    Write-Host "Failed to delete namespace. There might be an issue with kubectl." -ForegroundColor Red
}
else {
    Write-Host "Successfully deleted all resources." -ForegroundColor Green
}

# Wait for namespace to be fully deleted
$attempts = 0
$maxAttempts = 10
$namespaceExists = $true

Write-Host "Waiting for namespace to be fully deleted..." -ForegroundColor Yellow
while ($namespaceExists -and $attempts -lt $maxAttempts) {
    $attempts++
    Start-Sleep -Seconds 2
    
    # Use try-catch to handle the "not found" case gracefully
    try {
        $result = kubectl get namespace spidey-senses-local -o name 2>&1
        if ($LASTEXITCODE -ne 0) {
            # If kubectl returns non-zero, namespace is gone
            $namespaceExists = $false
            Write-Host "Namespace has been deleted." -ForegroundColor Green
            break
        }
    } catch {
        # If an error occurs, assume namespace is gone
        $namespaceExists = $false
        Write-Host "Namespace has been deleted." -ForegroundColor Green
        break
    }
    
    Write-Host "Namespace still being deleted, waiting... (attempt $attempts of $maxAttempts)" -ForegroundColor Yellow
}

if ($namespaceExists) {
    Write-Host "Namespace could not be deleted within the timeout period. You might need to delete it manually later." -ForegroundColor Red
}