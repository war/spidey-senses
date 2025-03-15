# Deploy to local Kubernetes cluster

$ErrorActionPreference = "Stop"

# Check if kubectl is installed
try {
    kubectl version --client | Out-Null
}
catch {
    Write-Host "kubectl not found. Please install kubectl and configure it for your Docker Desktop Kubernetes cluster." -ForegroundColor Red
    exit 1
}

# Check if Docker Desktop Kubernetes is running
try {
    $context = kubectl config current-context
    if ($context -ne "docker-desktop") {
        Write-Host "Current Kubernetes context is $context. Switching to docker-desktop..." -ForegroundColor Yellow
        kubectl config use-context docker-desktop
        if (-not $?) {
            Write-Host "Failed to switch to docker-desktop context. Please make sure Docker Desktop Kubernetes is enabled." -ForegroundColor Red
            exit 1
        }
    }
}
catch {
    Write-Host "Failed to get Kubernetes context. Please make sure Docker Desktop Kubernetes is enabled." -ForegroundColor Red
    exit 1
}

# Install NGINX Ingress Controller if not already installed
Write-Host "Checking for NGINX Ingress Controller..." -ForegroundColor Green
$nginxPods = kubectl get pods -n ingress-nginx -l app.kubernetes.io/name=ingress-nginx -o name 2>$null
if (-not $nginxPods) {
    Write-Host "NGINX Ingress Controller not found. Installing..." -ForegroundColor Yellow
    
    # Add the ingress-nginx repository
    helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
    helm repo update
    
    # Install the ingress-nginx chart
    helm install ingress-nginx ingress-nginx/ingress-nginx `
        --create-namespace `
        --namespace ingress-nginx
        
    if (-not $?) {
        Write-Host "Failed to install NGINX Ingress Controller" -ForegroundColor Red
        exit 1
    }
    
    # Wait for the controller to be ready
    Write-Host "Waiting for NGINX Ingress Controller to be ready..." -ForegroundColor Yellow
    kubectl wait --namespace ingress-nginx `
        --for=condition=ready pod `
        --selector=app.kubernetes.io/name=ingress-nginx `
        --timeout=90s
}
else {
    Write-Host "NGINX Ingress Controller is already installed" -ForegroundColor Green
}

# Verify directory structure
$basePath = "./infra/k8s/base"
$overlayPath = "./infra/k8s/overlays/local"

if (-not (Test-Path $basePath)) {
    Write-Host "Base directory $basePath not found!" -ForegroundColor Red
    exit 1
}

if (-not (Test-Path "$basePath/kustomization.yaml")) {
    Write-Host "Base kustomization.yaml not found in $basePath!" -ForegroundColor Red
    exit 1
}

if (-not (Test-Path $overlayPath)) {
    Write-Host "Overlay directory $overlayPath not found!" -ForegroundColor Red
    exit 1
}

if (-not (Test-Path "$overlayPath/kustomization.yaml")) {
    Write-Host "Overlay kustomization.yaml not found in $overlayPath!" -ForegroundColor Red
    exit 1
}

# Validate kustomization
Write-Host "Validating kustomization..." -ForegroundColor Green
$kustomizeOutput = kubectl kustomize $overlayPath 2>&1
if (-not $?) {
    Write-Host "Kustomization validation failed:" -ForegroundColor Red
    Write-Host $kustomizeOutput -ForegroundColor Red
    exit 1
}

# Apply Kubernetes manifests using kustomize
Write-Host "Deploying to local Kubernetes cluster..." -ForegroundColor Green
kubectl apply -k $overlayPath

if (-not $?) {
    Write-Host "Failed to deploy to Kubernetes" -ForegroundColor Red
    exit 1
}

# Add local DNS entries to hosts file
Write-Host "Note: You may need to add the following entries to your hosts file:" -ForegroundColor Yellow
Write-Host "127.0.0.1 spidey-senses.local" -ForegroundColor Cyan
Write-Host "127.0.0.1 api.spidey-senses.local" -ForegroundColor Cyan

# Print service URLs
Write-Host "`nService URLs:" -ForegroundColor Green
Write-Host "Angular: http://spidey-senses.local" -ForegroundColor Cyan
Write-Host "API: http://api.spidey-senses.local" -ForegroundColor Cyan
Write-Host "API direct access: http://localhost:30080" -ForegroundColor Cyan
Write-Host "Angular direct access: http://localhost:30090" -ForegroundColor Cyan

Write-Host "`nTo check the status of your deployments:" -ForegroundColor Green
Write-Host "kubectl get all -n spidey-senses-local" -ForegroundColor Cyan