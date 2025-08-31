variable "resource_group_name" {
  description = "The name of the resource group"
  type        = string
}

variable "domain_name" {
  description = "The domain name for DNS zone"
  type        = string
}

variable "environment" {
  description = "Environment name (dev, prod, etc.)"
  type        = string
}

variable "ingress_controller_ip" {
  description = "The IP address for the Ingress Controller"
  type        = string
}