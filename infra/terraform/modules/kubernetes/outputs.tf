output "ingress_controller_ip" {
  description = "IP address for the Ingress Controller"
  value       = data.kubernetes_service.ingress_controller.status.0.load_balancer.0.ingress.0.ip
}

output "namespace" {
  description = "The Kubernetes namespace where resources are deployed"
  value       = kubernetes_namespace.spidey.metadata[0].name
}

output "api_service_name" {
  description = "Name of the API service"
  value       = kubernetes_service.api.metadata[0].name
}

output "ui_service_name" {
  description = "Name of the UI service"
  value       = kubernetes_service.ui.metadata[0].name
}