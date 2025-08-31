terraform {
  required_version = "~> 1.11.0"
  
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.95.0"
    }
    kubernetes = {
      source  = "hashicorp/kubernetes"
      version = "~> 2.27.0"
    }
    helm = {
      source  = "hashicorp/helm"
      version = "~> 2.12.0"
    }
  }
  
  backend "azurerm" {
    resource_group_name  = "spidey-terraform-state"
    storage_account_name = "spideytfstate"
    container_name       = "tfstate"
    key                  = "dev.terraform.tfstate"
  }
}

provider "azurerm" {
  features {
    resource_group {
      prevent_deletion_if_contains_resources = false
    }
    key_vault {
      purge_soft_delete_on_destroy = true
    }
  }
}

# This data source fetches information about the Azure subscription
data "azurerm_subscription" "current" {}

resource "azurerm_resource_group" "rg" {
  name     = "spidey-dev-rg"
  location = var.location
  
  tags = {
    Environment = "Development"
    Application = "SpideySenses"
  }
}

module "aks" {
  source = "../../modules/aks"
  
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  cluster_name        = "spidey-dev-aks"
  kubernetes_version  = "1.29"
  node_count          = 2
  vm_size             = "Standard_D2s_v3"
  environment         = "dev"
}

module "dns" {
  source = "../../modules/dns"
  
  resource_group_name = azurerm_resource_group.rg.name
  domain_name         = "spidey-senses.com"
  environment         = "dev"
  ingress_controller_ip = module.kubernetes.ingress_controller_ip
}

module "kubernetes" {
  source = "../../modules/kubernetes"
  
  host                   = module.aks.host
  client_certificate     = base64decode(module.aks.client_certificate)
  client_key             = base64decode(module.aks.client_key)
  cluster_ca_certificate = base64decode(module.aks.cluster_ca_certificate)
  
  api_image_name = var.api_image
  ui_image_name  = var.ui_image

  api_replicas = 2
  ui_replicas  = 2
  
  environment  = "dev"
  domain_name  = "spidey-senses.com"
  github_token = var.github_token
}
