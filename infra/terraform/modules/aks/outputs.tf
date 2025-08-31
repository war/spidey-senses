output "cluster_id" {
  description = "The ID of the AKS cluster"
  value       = azurerm_kubernetes_cluster.cluster.id
}

output "cluster_name" {
  description = "The name of the AKS cluster"
  value       = azurerm_kubernetes_cluster.cluster.name
}

output "host" {
  description = "The Kubernetes cluster server host"
  value       = azurerm_kubernetes_cluster.cluster.kube_config.0.host
  sensitive   = true
}

output "client_certificate" {
  description = "Base64 encoded client certificate for authentication"
  value       = azurerm_kubernetes_cluster.cluster.kube_config.0.client_certificate
  sensitive   = true
}

output "client_key" {
  description = "Base64 encoded client key for authentication"
  value       = azurerm_kubernetes_cluster.cluster.kube_config.0.client_key
  sensitive   = true
}

output "cluster_ca_certificate" {
  description = "Base64 encoded CA certificate for the cluster"
  value       = azurerm_kubernetes_cluster.cluster.kube_config.0.cluster_ca_certificate
  sensitive   = true
}

output "kubeconfig" {
  description = "Raw kubeconfig file for the cluster"
  value       = azurerm_kubernetes_cluster.cluster.kube_config_raw
  sensitive   = true
}

output "log_analytics_workspace_id" {
  description = "ID of the Log Analytics Workspace"
  value       = azurerm_log_analytics_workspace.aks.id
}