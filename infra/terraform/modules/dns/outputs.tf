output "dns_zone_name" {
  description = "The name of the DNS zone"
  value       = azurerm_dns_zone.main.name
}

output "dns_zone_id" {
  description = "The ID of the DNS zone"
  value       = azurerm_dns_zone.main.id
}

output "name_servers" {
  description = "The name servers for the DNS zone"
  value       = azurerm_dns_zone.main.name_servers
}

output "api_fqdn" {
  description = "The fully qualified domain name for the API"
  value       = "${azurerm_dns_a_record.api.name}.${azurerm_dns_zone.main.name}"
}

output "ui_fqdn" {
  description = "The fully qualified domain name for the UI"
  value       = var.environment == "prod" ? var.domain_name : "${var.environment}.${var.domain_name}"
}