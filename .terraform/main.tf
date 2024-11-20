terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "4.10.0"
    }
  }
}

provider "azurerm" {
  subscription_id = var.subscription_id
  tenant_id       = var.tenant_id
  features {

  }
}

resource "azurerm_resource_group" "rg" {
  name     = var.resource_group_name
  location = var.region
  tags     = local.tags
}
