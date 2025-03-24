variable "location" {
  description = "The Azure region where resources will be created"
  type        = string
  default     = "UK South"
}

variable "api_image" {
  description = "The full name of the API Docker image including tag"
  type        = string
  default     = "ghcr.io/war/spidey-senses/web-api-v2:develop"
}

variable "ui_image" {
  description = "The full name of the UI Docker image including tag"
  type        = string
  default     = "ghcr.io/war/spidey-senses/angular-js:develop"
}

variable "github_token" {
  description = "GitHub Personal Access Token for pulling images from GHCR"
  type        = string
  sensitive   = true
}
