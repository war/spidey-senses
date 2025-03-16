# deploy-local.ps1
#
# This script cleans up any existing deployment and deploys the spidey-senses
# application to your local Kubernetes cluster using the local overlay

# Set error action preference to stop on any error
$ErrorActionPreference = "Stop"

# Variables
$namespace = "spidey-senses"
$frontendImageName = "spidey-senses/angular-js"
$frontendImageTag = "local"
$apiImageName = "spidey-senses/web-api-v2"
$apiImageTag = "local"

$frontendFullImage = "$frontendImageName`:$frontendImageTag"
$apiFullImage = "$apiImageName`:$apiImageTag"

try {
    $kubeContext = kubectl config current-context
    Write-Host "=========================================================="
    Write-Host "Deploying spidey-senses to Kubernetes - Local Environment"
    Write-Host "Using Kubernetes context: $kubeContext"
    Write-Host "=========================================================="
} catch {
    Write-Host "=========================================================="
    Write-Host "Deploying spidey-senses to Kubernetes - Local Environment"
    Write-Host "Could not determine current Kubernetes context"
    Write-Host "=========================================================="
}

# Check if kubectl is installed
try {
    kubectl version --client | Out-Null
    Write-Host "[OK] kubectl is installed" -ForegroundColor Green
} catch {
    Write-Host "[ERROR] kubectl is not installed. Please install kubectl first." -ForegroundColor Red
    exit 1
}

# Check if kustomize is available (it's included with recent kubectl versions)
try {
    kubectl kustomize --help | Out-Null
    Write-Host "[OK] kustomize is available" -ForegroundColor Green
} catch {
    Write-Host "[ERROR] kustomize is not available. Please install a newer version of kubectl." -ForegroundColor Red
    exit 1
}

# Function to check if namespace exists
function Test-NamespaceExists {
    param (
        [string]$Namespace
    )
    
    $result = kubectl get namespace $Namespace --ignore-not-found -o name
    return $result -ne ""
}

# Function to delete namespace and wait for it to be fully deleted
function Remove-NamespaceAndWait {
    param (
        [string]$Namespace
    )
    
    Write-Host "Deleting namespace $Namespace..." -ForegroundColor Yellow
    kubectl delete namespace $Namespace --wait=true
    
    # Wait for the namespace to be fully deleted
    $retries = 0
    $maxRetries = 30
    while ((Test-NamespaceExists -Namespace $Namespace) -and ($retries -lt $maxRetries)) {
        Write-Host "Waiting for namespace $Namespace to be deleted... ($retries/$maxRetries)" -ForegroundColor Yellow
        Start-Sleep -Seconds 2
        $retries++
    }
    
    if (Test-NamespaceExists -Namespace $Namespace) {
        Write-Host "[ERROR] Timed out waiting for namespace $Namespace to be deleted." -ForegroundColor Red
        exit 1
    } else {
        Write-Host "[OK] Namespace $Namespace deleted" -ForegroundColor Green
    }
}

# Check if local images exist
Write-Host "Checking for local Docker images..." -ForegroundColor Yellow

try {
    $frontendExists = docker image inspect $frontendFullImage 2>$null
    if ($frontendExists) {
        Write-Host "[OK] Frontend image '$frontendFullImage' found" -ForegroundColor Green
    } else {
        Write-Host "[WARNING] Frontend image '$frontendFullImage' not found locally." -ForegroundColor Yellow
        Write-Host "  You can build it with: docker build -t $frontendFullImage ./src/frontend" -ForegroundColor Yellow
    }
} catch {
    Write-Host "[WARNING] Could not check for frontend image: $_" -ForegroundColor Yellow
}

try {
    $apiExists = docker image inspect $apiFullImage 2>$null
    if ($apiExists) {
        Write-Host "[OK] API image '$apiFullImage' found" -ForegroundColor Green
    } else {
        Write-Host "[WARNING] API image '$apiFullImage' not found locally." -ForegroundColor Yellow
        Write-Host "  You can build it with: docker build -t $apiFullImage ./src/api" -ForegroundColor Yellow
    }
} catch {
    Write-Host "[WARNING] Could not check for API image: $_" -ForegroundColor Yellow
}

# Clean up existing deployment if namespace exists
if (Test-NamespaceExists -Namespace $namespace) {
    Write-Host "Found existing namespace $namespace" -ForegroundColor Yellow
    $cleanupChoice = Read-Host "Do you want to clean up the existing deployment? (y/n)"
    
    if ($cleanupChoice -eq "y") {
        Remove-NamespaceAndWait -Namespace $namespace
    } else {
        Write-Host "Skipping cleanup, will update existing deployment" -ForegroundColor Yellow
    }
}

# Create namespace if it doesn't exist
if (-not (Test-NamespaceExists -Namespace $namespace)) {
    Write-Host "Creating namespace $namespace..." -ForegroundColor Yellow
    kubectl create namespace $namespace
    Write-Host "[OK] Namespace $namespace created" -ForegroundColor Green
}

# Apply the kustomization
Write-Host "Deploying spidey-senses with local overlay..." -ForegroundColor Yellow
try {
    kubectl apply -k ./infra/k8s/overlays/local
    Write-Host "[OK] Deployment successful" -ForegroundColor Green
} catch {
    Write-Host "[ERROR] Deployment failed: $_" -ForegroundColor Red
    exit 1
}

# Wait for pods to be ready
Write-Host "Waiting for pods to be ready..." -ForegroundColor Yellow
try {
    kubectl wait --namespace=$namespace --for=condition=Ready pods --all --timeout=120s
    Write-Host "[OK] All pods are ready" -ForegroundColor Green
} catch {
    Write-Host "[WARNING] Not all pods are ready within the timeout period: $_" -ForegroundColor Yellow
}

# Get pod status
Write-Host "`nPod Status:" -ForegroundColor Cyan
kubectl get pods -n $namespace

# Get service details
Write-Host "`nService Details:" -ForegroundColor Cyan
kubectl get svc -n $namespace

# Get ingress details
Write-Host "`nIngress Details:" -ForegroundColor Cyan
kubectl get ingress -n $namespace

Write-Host "`n=========================================================="
Write-Host "[OK] Deployment complete!" -ForegroundColor Green
Write-Host "Access your application at:"
Write-Host "  Frontend: http://spidey-senses.local/"
Write-Host "  API:      http://api.spidey-senses.local/"
Write-Host "=========================================================="
