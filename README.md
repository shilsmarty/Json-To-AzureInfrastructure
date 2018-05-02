# Json-To-AzureInfrastructure
* Provision resources (webapp , storage , keyvault etc.) in azure as an input Json file.
* Json file has to follow certain pattern (sample json file attached).
* After validation of pattern , infrastructure deployment will initiate .
* For Authentication need setup ServicePrincipal in Azure AD (app registration) and this serviceprincipal should be contributor on that azure subscription.
