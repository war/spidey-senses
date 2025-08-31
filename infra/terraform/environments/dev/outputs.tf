output "resource_group_name" {
  description = "Name of the created resource group"
  value       = azurerm_resource_group.rg.name
}

output "aks_cluster_name" {
  description = "Name of the AKS cluster"
  value       = module.aks.cluster_name
}

output "aks_cluster_id" {
  description = "ID of the AKS cluster"
  value       = module.aks.cluster_id
}

output "kubeconfig" {
  description = "kubeconfig file for connecting to the cluster"
  value       = module.aks.kubeconfig
  sensitive   = true
}

output "api_endpoint" {
  description = "The URL for accessing the API"
  value       = "http://dev-api.spidey-senses.com"
}

output "ui_endpoint" {
  description = "The URL for accessing the UI"
  value       = "http://dev.spidey-senses.com"
}
