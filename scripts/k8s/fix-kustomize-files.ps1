# Script to fix kustomization patch issues

# Check if kubectl is installed
try {
    kubectl version --client | Out-Null
}
catch {
    Write-Host "kubectl not found. Please install kubectl." -ForegroundColor Red
    exit 1
}

# Directory path
$basePath = "./infra/k8s/base"
$localOverlayPath = "./infra/k8s/overlays/local"

# Validate kustomization - verbose mode to see exactly what's happening
Write-Host "Validating local kustomization (verbose mode)..." -ForegroundColor Green
kubectl kustomize --enable-helm --reorder=none $localOverlayPath --debug=true

# If there were errors, try a backup approach
if (-not $?) {
    Write-Host "Kustomization validation failed. Trying to fix issues..." -ForegroundColor Yellow
    
    # Create a simple kustomization file for troubleshooting
    @"
apiVersion: kustomize.config.k8s.io/v1beta1
kind: Kustomization

namespace: spidey-senses-local

resources:
- namespace.yaml
- configmap.yaml
- web-api-deployment-patch.yaml  # Using the patch directly as a resource
- angular-deployment-patch.yaml
- web-api-service-patch.yaml
- angular-service-patch.yaml
- ingress-patch.yaml

images:
- name: \${ACR_NAME}.azurecr.io/spidey-senses/web-api-v2
  newName: spidey-senses/web-api-v2
  newTag: local
- name: \${ACR_NAME}.azurecr.io/spidey-senses/angular-js
  newName: spidey-senses/angular-js
  newTag: local
"@ | Out-File -FilePath "$localOverlayPath/kustomization.yaml.simple" -Encoding utf8
    
    Write-Host "Created a simplified kustomization file for troubleshooting." -ForegroundColor Yellow
    Write-Host "Test with: kubectl kustomize ./infra/k8s/overlays/local/ --kustomization-file=kustomization.yaml.simple" -ForegroundColor Cyan
}

Write-Host "`nIf you're still having issues, try applying resources individually:" -ForegroundColor Yellow
Write-Host "kubectl apply -f $localOverlayPath/namespace.yaml" -ForegroundColor Cyan
Write-Host "kubectl apply -f $localOverlayPath/configmap.yaml" -ForegroundColor Cyan
Write-Host "kubectl apply -f $localOverlayPath/web-api-deployment-patch.yaml" -ForegroundColor Cyan
Write-Host "kubectl apply -f $localOverlayPath/angular-deployment-patch.yaml" -ForegroundColor Cyan
Write-Host "..." -ForegroundColor Cyan