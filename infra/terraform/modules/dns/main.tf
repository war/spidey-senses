# Create a DNS zone if it doesn't already exist
# TODO: Maybe have seperate config for DNS?
resource "azurerm_dns_zone" "main" {
  name                = var.domain_name
  resource_group_name = var.resource_group_name
  
  tags = {
    Environment = var.environment
    Application = "SpideySenses"
  }
  
  lifecycle {
    prevent_destroy = true
  }
}

# API A record
resource "azurerm_dns_a_record" "api" {
  name                = var.environment == "prod" ? "api" : "${var.environment}-api"
  zone_name           = azurerm_dns_zone.main.name
  resource_group_name = var.resource_group_name
  ttl                 = 300
  records             = [var.ingress_controller_ip]
  
  tags = {
    Environment = var.environment
    Application = "SpideySenses"
  }
}

# UI A record
resource "azurerm_dns_a_record" "ui" {
  name                = var.environment == "prod" ? "@" : var.environment
  zone_name           = azurerm_dns_zone.main.name
  resource_group_name = var.resource_group_name
  ttl                 = 300
  records             = [var.ingress_controller_ip]
  
  tags = {
    Environment = var.environment
    Application = "SpideySenses"
  }
}

# www CNAME record for the UI
resource "azurerm_dns_cname_record" "www" {
  name                = var.environment == "prod" ? "www" : "www-${var.environment}"
  zone_name           = azurerm_dns_zone.main.name
  resource_group_name = var.resource_group_name
  ttl                 = 300
  record              = var.environment == "prod" ? var.domain_name : "${var.environment}.${var.domain_name}"
  
  tags = {
    Environment = var.environment
    Application = "SpideySenses"
  }
}