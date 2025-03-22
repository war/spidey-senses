$ErrorActionPreference = "Stop"

try {
    kubectl version --client | Out-Null
} catch {
    Write-Host "kubectl not found. Please install kubectl and configure it for your Docker Desktop Kubernetes cluster." -ForegroundColor Red
    exit 1
}

$namespace = "spidey-senses-local"
$namespaceExists = $false

try {
    $result = kubectl get namespace $namespace -o name 2>&1
    if ($LASTEXITCODE -eq 0) {
        $namespaceExists = $true
    }
} catch {
    $namespaceExists = $false
}

if (-not $namespaceExists) {
    Write-Host "Namespace $namespace does not exist. Please deploy your application first." -ForegroundColor Red
    exit 1
}

Write-Host "`nChecking NGINX Ingress Controller status:" -ForegroundColor Green
kubectl get pods -n ingress-nginx

$ingressControllerRunning = $false
try {
    $pods = kubectl get pods -n ingress-nginx -l app.kubernetes.io/name=ingress-nginx -o jsonpath='{.items[0].status.phase}'
    if ($pods -eq "Running") {
        $ingressControllerRunning = $true
    }
} catch {
    $ingressControllerRunning = $false
}

if (-not $ingressControllerRunning) {
    Write-Host "`nWARNING: NGINX Ingress Controller is not running. Ingress resources won't work." -ForegroundColor Red
    Write-Host "Try reinstalling the NGINX Ingress Controller:" -ForegroundColor Yellow
    Write-Host "helm install ingress-nginx ingress-nginx/ingress-nginx --create-namespace --namespace ingress-nginx" -ForegroundColor Cyan
}

Write-Host "`nChecking Ingress resources:" -ForegroundColor Green
kubectl get ingress -n $namespace

Write-Host "`nChecking Services:" -ForegroundColor Green
kubectl get services -n $namespace

Write-Host "`nChecking Deployments:" -ForegroundColor Green
kubectl get deployments -n $namespace

Write-Host "`nChecking Pods:" -ForegroundColor Green
kubectl get pods -n $namespace

$podStatus = kubectl get pods -n $namespace -o jsonpath='{.items[*].status.phase}'
if ($podStatus -match "Failed|Error|CrashLoopBackOff") {
    Write-Host "`nFound pods with issues. Checking logs:" -ForegroundColor Yellow
    
    $pods = kubectl get pods -n $namespace -o jsonpath='{.items[*].metadata.name}'
    foreach ($pod in $pods.Split(' ')) {
        Write-Host "`nLogs for pod $pod :" -ForegroundColor Cyan
        kubectl logs -n $namespace $pod
    }
}

Write-Host "`nYou can access your services directly via these ports:" -ForegroundColor Green
$webApiNodePort = kubectl get service local-web-api-v2 -n $namespace -o jsonpath='{.spec.ports[0].nodePort}' 2>$null
$angularNodePort = kubectl get service local-angular-js -n $namespace -o jsonpath='{.spec.ports[0].nodePort}' 2>$null

if ($webApiNodePort) {
    Write-Host "  API: http://localhost:$webApiNodePort" -ForegroundColor Cyan
}

if ($angularNodePort) {
    Write-Host "  Frontend: http://localhost:$angularNodePort" -ForegroundColor Cyan
}

Write-Host "`nIf you've updated your hosts file, you should also be able to access:" -ForegroundColor Green
Write-Host "  Frontend: http://spidey-senses.local" -ForegroundColor Cyan
Write-Host "  API: http://api.spidey-senses.local" -ForegroundColor Cyan
