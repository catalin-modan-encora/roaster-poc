## Prerequisites
1. Azure CLI installed locally.
2. Terraform CLI installed locally. Scoop is a good choice as a package manager.
3. An active Azure Subscription.
   1. `Owner` or `Contributor` + `User Access Administrator` roles over the subscription.
   2. Set up a budget to prevent unwanted costs.
4. VS-Code installed locally.
5. GIT installed locally.

## Setup
1. Create your variables overrides for local development.
   1. Copy `example.variables_override.tf` into `variables_override.tf`.
2. Login using Azure CLI: `az login --tenant variables_override.tf`.
   1. Also set the current subscription using: `az account set --subscription <ID>`.
3. Preview actions which need to be applied: `terraform plan`.
4. Create the environment: `terraform apply`
.
## Useful commands
1. Initialize: `terraform init`
2. Format: `terraform format`
3. Plan: `terraform plan`
4. Apply: `terraform apply`