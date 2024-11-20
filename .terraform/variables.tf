variable "subscription_id" {
  description = "The subscription ID for the deployment."
  type        = string
}

variable "tenant_id" {
  description = "The tenant ID where the subscription lives."
  type        = string
}

variable "resource_group_name" {
  default     = "rg-roaster-northeu"
  description = "The name of the resource group containing the resources."
  type        = string
}

variable "region" {
  default     = "northeu"
  description = "The region where the resources will be deployed."
  type        = string
}
