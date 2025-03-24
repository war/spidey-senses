variable "resource_group_name" {
  description = "Name of the resource group"
  type        = string
}

variable "location" {
  description = "Azure region where the cluster will be deployed"
  type        = string
}

variable "cluster_name" {
  description = "Name of the AKS cluster"
  type        = string
}

variable "kubernetes_version" {
  description = "Kubernetes version to use"
  type        = string
  default     = "1.29"
}

variable "node_count" {
  description = "Number of worker nodes in the cluster"
  type        = number
  default     = 2
}

variable "vm_size" {
  description = "VM size for the nodes"
  type        = string
  default     = "Standard_D2s_v3"
}

variable "environment" {
  description = "Environment name (dev, prod, etc.)"
  type        = string
}