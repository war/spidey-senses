# Script to check the status of the ingress and services

$ErrorActionPreference = "Stop"

# Check if kubectl is installed
try {
    kubectl version --client | Out-Null
}
catch {
    Write-Host "kubectl not found. Please install kubectl and configure it for your Docker Desktop Kubernetes cluster." -ForegroundColor Red
    exit 1
}

# Check for namespace existence
$namespace = "spidey-senses"
$namespaceExists = $false

try {
    $result = kubectl get namespace $namespace -o name 2>&1
    if ($LASTEXITCODE -eq 0) {
        $namespaceExists = $true
    }
}
catch {
    $namespaceExists = $false
}

if (-not $namespaceExists) {
    Write-Host "Namespace $namespace does not exist. Please deploy your application first." -ForegroundColor Red
    exit 1
}

# Check NGINX Ingress Controller
Write-Host "`nChecking NGINX Ingress Controller status:" -ForegroundColor Green
kubectl get pods -n ingress-nginx

# Check if ingress-nginx is running and ready
$ingressControllerRunning = $false
try {
    $pods = kubectl get pods -n ingress-nginx -l app.kubernetes.io/name=ingress-nginx -o jsonpath='{.items[0].status.phase}'
    if ($pods -eq "Running") {
        $ingressControllerRunning = $true
    }
}
catch {
    $ingressControllerRunning = $false
}

if (-not $ingressControllerRunning) {
    Write-Host "`nWARNING: NGINX Ingress Controller is not running. Ingress resources won't work." -ForegroundColor Red
    Write-Host "Try reinstalling the NGINX Ingress Controller:" -ForegroundColor Yellow
    Write-Host "helm install ingress-nginx ingress-nginx/ingress-nginx --create-namespace --namespace ingress-nginx" -ForegroundColor Cyan
}

# Check ingress resources
Write-Host "`nChecking Ingress resources:" -ForegroundColor Green
kubectl get ingress -n $namespace

# Check services
Write-Host "`nChecking Services:" -ForegroundColor Green
kubectl get services -n $namespace

# Check deployments and pods
Write-Host "`nChecking Deployments:" -ForegroundColor Green
kubectl get deployments -n $namespace

Write-Host "`nChecking Pods:" -ForegroundColor Green
kubectl get pods -n $namespace

# Check pod logs if there are issues
$podStatus = kubectl get pods -n $namespace -o jsonpath='{.items[*].status.phase}'
if ($podStatus -match "Failed|Error|CrashLoopBackOff") {
    Write-Host "`nFound pods with issues. Checking logs:" -ForegroundColor Yellow
    
    $pods = kubectl get pods -n $namespace -o jsonpath='{.items[*].metadata.name}'
    foreach ($pod in $pods.Split(' ')) {
        Write-Host "`nLogs for pod $pod :" -ForegroundColor Cyan
        kubectl logs -n $namespace $pod
    }
}

# Check node ports
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

Write-Host "`nIf you can't access these URLs, make sure:" -ForegroundColor Yellow
Write-Host "1. You've added the entries to your hosts file (run scripts/k8s/update-hosts.ps1 as Administrator)" -ForegroundColor Yellow
Write-Host "2. The NGINX Ingress Controller is running" -ForegroundColor Yellow
Write-Host "3. Your pods are in the 'Running' state" -ForegroundColor Yellow
Write-Host "4. You've built and tagged your Docker images correctly" -ForegroundColor Yellow