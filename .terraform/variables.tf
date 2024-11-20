locals {
  # Tags which need to be applied to all resources
  tags = {
    ProjectName = "Roaster"
    Environment = "Roaster-DEV"
    Owner       = var.owner
  }
  # Enforce naming conventions
  resource_group_name = "rg-roaster-${var.region}"
}

variable "subscription_id" {
  description = "The subscription ID for the deployment."
  type        = string
}

variable "tenant_id" {
  description = "The tenant ID where the subscription lives."
  type        = string
}

variable "owner" {
  description = "The owner of the environment"
  type        = string
}

variable "region" {
  description = "The region where the resources will be deployed."
  type        = string
  validation {
    condition     = contains(["eastus", "southcentralus", "westus2", "westus3", "australiaeast", "southeastasia", "northeurope", "swedencentral", "uksouth", "westeurope", "centralus", "southafricanorth", "centralindia", "eastasia", "japaneast", "koreacentral", "newzealandnorth", "canadacentral", "francecentral", "germanywestcentral", "italynorth", "norwayeast", "polandcentral", "spaincentral", "switzerlandnorth", "mexicocentral", "uaenorth", "brazilsouth", "israelcentral", "qatarcentral", "centralusstage", "eastusstage", "eastus2stage", "northcentralusstag", "southcentralusstag", "westusstage", "westus2stage", "asia", "asiapacific", "australia", "brazil", "canada", "europe", "france", "germany", "global", "india", "israel", "italy", "japan", "korea", "newzealand", "norway", "poland", "qatar", "singapore", "southafrica", "sweden", "switzerland", "uae", "uk", "unitedstates", "unitedstateseuap", "eastasiastage", "southeastasiastage", "brazilus", "eastus2", "eastusstg", "northcentralus", "westus", "japanwest", "jioindiawest", "centraluseuap", "eastus2euap", "southcentralusstg", "westcentralus", "southafricawest", "australiacentral", "australiacentral2", "australiasoutheast", "jioindiacentral", "koreasouth", "southindia", "westindia", "canadaeast", "francesouth", "germanynorth", "norwaywest", "switzerlandwest", "ukwest", "uaecentral", "brazilsoutheast"], var.region)
    error_message = "Use 'az account list-locations -o table' to select a correct location."
  }
}
