using AzureInfraProvisioning.Model;
using System.Collections.Generic;

namespace AzureInfraProvisioning.Mapping
{
    public class ResourceMapping
    {
        private readonly ModelConverter.ModelConverter _converter = new ModelConverter.ModelConverter();

        public Dictionary<string, Dictionary<string, string>> GetResourceProperties(ProvisioningRequest request)
        {
            var resourceBag = new Dictionary<string, Dictionary<string,string>>();

            if (request?.Provisioning == null) return resourceBag;

            foreach (var resource in request.Provisioning?.Resources)
            {
                string mappedResource = null;
                switch (resource.Name)
                {
                    case "vm":
                    case "virtualMachine":
                    case "VM":
                    case "Virtual Machine":
                    {
                        mappedResource = "VM-Vnet-Subnet";
                        break;
                    }
                    case "webapp":
                    case "website":
                    case "app":
                    case "WebApp":
                    {
                        mappedResource = "WebApp";
                        break;
                    }
                    case "storage":
                    case "azurestorage":
                    {
                        mappedResource = "Storage";
                        break;
                    }
                    case "kv":
                    case "keyvault":
                    case "KeyVault":
                        {
                        mappedResource = "KeyVault";
                        break;
                    }
                    default:
                        break;
                }

                if (mappedResource != null)
                {
                    var propertyBag = _converter.GetPropertyBag(resource.Properties);
                    var resourceProperties = GetResourceProperties(mappedResource, propertyBag);
                    resourceBag.Add(resourceProperties.Key, resourceProperties.Value);
                }
            }

            var generalProperties = GetGeneralProperties(request);
            resourceBag.Add(generalProperties.Key, generalProperties.Value);

            return resourceBag;
        }

        private KeyValuePair<string, Dictionary<string,string>> GetResourceProperties(string resource , Dictionary<string,string> properties)
        {
            return new KeyValuePair<string, Dictionary<string, string>>(resource , properties);
        }

        private KeyValuePair<string, Dictionary<string, string>> GetGeneralProperties(ProvisioningRequest request)
        {
            var requestProperties = request?.Provisioning;
            var generalPropDictionary = new Dictionary<string, string>
            {
                {"ResourceGroupName", requestProperties?.resourcegroup},
                {"ResourceGroupLocation", "westus2"},
                {"SubscriptionId", requestProperties?.subscription_id}
            };

            return new KeyValuePair<string, Dictionary<string, string>>("general", generalPropDictionary);
        }
    }
}
