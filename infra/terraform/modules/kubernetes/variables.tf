variable "host" {
  description = "The Kubernetes server host"
  type        = string
  sensitive   = true
}

variable "client_certificate" {
  description = "Client certificate for authentication"
  type        = string
  sensitive   = true
}

variable "client_key" {
  description = "Client key for authentication"
  type        = string
  sensitive   = true
}

variable "cluster_ca_certificate" {
  description = "CA certificate for the cluster"
  type        = string
  sensitive   = true
}

variable "api_image_name" {
  description = "The full name of the API Docker image to deploy"
  type        = string
}

variable "ui_image_name" {
  description = "The full name of the UI Docker image to deploy"
  type        = string
}

variable "api_replicas" {
  description = "Number of API pods to run"
  type        = number
  default     = 2
}

variable "ui_replicas" {
  description = "Number of UI pods to run"
  type        = number
  default     = 2
}

variable "environment" {
  description = "Environment name (dev, prod, etc.)"
  type        = string
}

variable "domain_name" {
  description = "Base domain name for the application"
  type        = string
}

variable "github_token" {
  description = "GitHub Personal Access Token for pulling images from GHCR"
  type        = string
  sensitive   = true
}