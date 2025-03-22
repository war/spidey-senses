$ErrorActionPreference = "Stop"

$namespace = "spidey-senses-local"
$frontendImageName = "spidey-senses/angular-js"
$frontendImageTag = "local"
$apiImageName = "spidey-senses/web-api-v2"
$apiImageTag = "local"

$frontendFullImage = "$frontendImageName`:$frontendImageTag"
$apiFullImage = "$apiImageName`:$apiImageTag"
$k8sDir = "./infra/k8s"

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

try {
    $null = kubectl version --client 2>$null
    Write-Host "[OK] kubectl is installed" -ForegroundColor Green
} catch {
    Write-Host "[ERROR] kubectl is not installed. Please install kubectl first." -ForegroundColor Red
    exit 1
}

try {
    kubectl kustomize --help | Out-Null
    Write-Host "[OK] kustomize is available" -ForegroundColor Green
} catch {
    Write-Host "[ERROR] kustomize is not available. Please install a newer version of kubectl." -ForegroundColor Red
    exit 1
}

function Test-NamespaceExists {
    param (
        [string]$Namespace
    )
    
    $result = kubectl get namespace $Namespace --ignore-not-found 2>$null
    return $result -ne $null -and $result -ne ""
}

function Remove-NamespaceAndWait {
    param (
        [string]$Namespace
    )
    
    Write-Host "Deleting namespace $Namespace..." -ForegroundColor Yellow
    kubectl delete namespace $Namespace --wait=true
    
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

if (Test-NamespaceExists -Namespace $namespace) {
    Write-Host "Found existing namespace $namespace" -ForegroundColor Yellow
    $cleanupChoice = Read-Host "Do you want to clean up the existing deployment? (y/n)"
    
    if ($cleanupChoice -eq "y") {
        Remove-NamespaceAndWait -Namespace $namespace
    } else {
        Write-Host "Skipping cleanup, will update existing deployment" -ForegroundColor Yellow
    }
}

$namespaceExists = Test-NamespaceExists -Namespace $namespace
if (-not $namespaceExists) {
    Write-Host "Creating namespace $namespace..." -ForegroundColor Yellow
    kubectl apply -f $k8sDir/base/common/namespace.yaml
    Write-Host "[OK] Namespace $namespace created" -ForegroundColor Green
} else {
    Write-Host "[OK] Namespace $namespace already exists" -ForegroundColor Green
}

Write-Host "Deploying spidey-senses with local overlay..." -ForegroundColor Yellow

try {
    Write-Host "Applying resources..." -ForegroundColor Yellow
    kubectl apply -k $k8sDir/overlays/local
    Write-Host "[OK] Deployment successful" -ForegroundColor Green
} catch {
    Write-Host "[ERROR] Deployment failed: $_" -ForegroundColor Red
    exit 1
}

Write-Host "Waiting for pods to be ready..." -ForegroundColor Yellow
try {
    kubectl wait --namespace=$namespace --for=condition=Ready pods --all --timeout=60s
    Write-Host "[OK] All pods are ready" -ForegroundColor Green
} catch {
    Write-Host "[WARNING] Not all pods are ready within the timeout period: $_" -ForegroundColor Yellow
}

Write-Host "`nPod Status:" -ForegroundColor Cyan
kubectl get pods -n $namespace

Write-Host "`nService Details:" -ForegroundColor Cyan
kubectl get svc -n $namespace

Write-Host "`nIngress Details:" -ForegroundColor Cyan
kubectl get ingress -n $namespace

Write-Host "`n=========================================================="
Write-Host "[OK] Deployment complete!" -ForegroundColor Green
Write-Host "Access your application at:"
Write-Host "  Frontend: http://spidey-senses.local/"
Write-Host "  API:      http://api.spidey-senses.local/"
Write-Host "=========================================================="
